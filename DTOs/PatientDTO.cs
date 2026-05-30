using Cw8.Models;

namespace Cw8.DTOs;

public class PatientDTO
{
    public string Pesel { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int Age { get; set; }
    public string Sex { get; set; }
    public ICollection<AdmissionDTO> Admissions;
    public ICollection<BedAssignmentDTO> bedAssignments;
}