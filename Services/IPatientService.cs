using Cw8.DTOs;

namespace Cw8.Services;

public interface IPatientService
{
    Task<ICollection<PatientDTO>> GetPatientsAsync(string search, CancellationToken cancellationToken);
    Task<ResponseBedAssignmentDTO> AddPatientAsync(string pesel, PostBedAssignmentDTO bedAssignment, CancellationToken cancellationToken);
    
}