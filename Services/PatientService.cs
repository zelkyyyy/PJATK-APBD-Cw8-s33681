using Cw8.DTOs;
using Cw8.Infrastructure;
using Cw8.Models;
using Microsoft.EntityFrameworkCore;

namespace Cw8.Services;

public class PatientService : IPatientService
{
    private readonly MasterContext _context;
    public PatientService(MasterContext context)
    {
        _context = context;
    }
    public async Task<ICollection<PatientDTO>> GetPatientsAsync(string search, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower();
        }
        else
        {
            search = "";
        }
        var patients = await _context.Patients.Where(e=> (
            e.LastName.ToLower().Contains(search) || 
            (e.FirstName.ToLower().Contains(search))
            )).Select(p => new PatientDTO
        {
            Pesel = p.Pesel,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Age = p.Age,
            Sex = p.Sex ? "Male" : "Female",
            Admissions = p.Admissions.Select(a => new AdmissionDTO
            {
                Id = a.Id,
                AdmissionDate = a.AdmissionDate,
                DischargeDate = a.DischargeDate,
                Ward = new WardDTO {
                    Id = a.Ward.Id,
                    Name = a.Ward.Name,
                    Description = a.Ward.Description
                }
            }).ToList(),
            bedAssignments = p.BedAssignments.Select(ba => new BedAssignmentDTO
            {
                Id = ba.Id,
                From = ba.From,
                To = ba.To,
                Bed = new BedDTO
                {
                    Id = ba.Bed.Id,
                    BedType = new BedTypeDTO
                    {
                        Id = ba.Bed.BedType.Id,
                        Name = ba.Bed.BedType.Name,
                        Description = ba.Bed.BedType.Description
                    },
                    Room = new RoomDTO
                    {
                        Id = ba.Bed.Room.Id,
                        HasTv =  ba.Bed.Room.HasTv,
                        Ward = new WardDTO
                        {
                            Id = ba.Bed.Room.Ward.Id,
                            Name = ba.Bed.Room.Ward.Name,
                            Description = ba.Bed.Room.Ward.Description
                        }
                    }
                }
            }).ToList()
        }).ToListAsync();
        
        return patients;
    }

    public async Task<ResponseBedAssignmentDTO> AddPatientAsync(string pesel, PostBedAssignmentDTO bedAssignment, CancellationToken cancellationToken)
    {
        var bed = await _context.Beds.Where(b => b.BedType.Name == bedAssignment.BedType && b.Room.Ward.Name==bedAssignment.Ward && !b.BedAssignments.Any(ba => 
            (bedAssignment.To == null || ba.From < bedAssignment.To) && 
            (ba.To == null || bedAssignment.From < ba.To)
        )).FirstOrDefaultAsync(cancellationToken);

        if (bed == null)
        {
            throw new Exception("Bed not found");
        }

        var newPatient = new BedAssignment
        {
            PatientPesel = pesel, BedId = bed.Id, From = bedAssignment.From, To = bedAssignment.To
        };
        await _context.BedAssignments.AddAsync(newPatient, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        
        return new ResponseBedAssignmentDTO
        {
            Id = newPatient.Id,
            PatientPesel = newPatient.PatientPesel,
            BedId = newPatient.BedId,
            From = newPatient.From,
            To = newPatient.To
        };
    }
}