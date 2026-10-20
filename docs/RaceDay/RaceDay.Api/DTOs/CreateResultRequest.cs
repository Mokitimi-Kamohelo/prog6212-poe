using System.ComponentModel.DataAnnotations;

namespace RaceDay.Api.DTOs;

public class CreateResultRequest
{
    [Range(1, int.MaxValue)]
    public int EnrolmentId { get; set; }

    [Required]
    public TimeOnly FinishTime { get; set; }

    [Range(1, int.MaxValue)]
    public int FinishPosition { get; set; }
}
