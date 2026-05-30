namespace Cw8.DTOs;

public class ResponseBedAssignmentDTO
{
        public int Id { get; set; }
        public string PatientPesel { get; set; }
        public int BedId { get; set; }
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
}