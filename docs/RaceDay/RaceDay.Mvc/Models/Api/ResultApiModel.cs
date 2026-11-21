namespace RaceDay.Mvc.Models.Api;

public class ResultApiModel
{
    public int ResultId { get; set; }

    public int EnrolmentId { get; set; }

    public TimeOnly? FinishTime { get; set; }

    public int? FinishPosition { get; set; }

    public string? EventName { get; set; }

    public string? ParticipantName { get; set; }
    public DateOnly EventDate { get; set; }
    public string? CategoryName { get; set; }
}