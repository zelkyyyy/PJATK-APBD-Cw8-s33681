using System.ComponentModel.DataAnnotations;

namespace Cw8.DTOs;

public class PostBedAssignmentDTO
{
    [Required]
    public DateTime From { get; set; }
    [Required]
    public DateTime? To { get; set; }
    [Required]
    public string BedType { get; set; }
    [Required]
    public string Ward { get; set; }
    
}