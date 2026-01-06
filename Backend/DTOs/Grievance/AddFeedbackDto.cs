
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Grievance;

public class AddFeedbackDto
{
    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    public string? Feedback { get; set; }
}
