using System.ComponentModel.DataAnnotations;

namespace RaceDay.Mvc.ViewModels;

public class CreateResultViewModel
{
    public int EnrolmentId { get; set; }

    public int EventId { get; set; }

    public string ParticipantName { get; set; } = "";

    public string EventName { get; set; } = "";

    [Required]
    [DataType(DataType.Time)]
    public TimeSpan FinishTime { get; set; }

    [Range(1, int.MaxValue)]
    public int FinishPosition { get; set; }
}