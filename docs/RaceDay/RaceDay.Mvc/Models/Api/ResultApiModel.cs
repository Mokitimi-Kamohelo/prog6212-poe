namespace RaceDay.Mvc.Models.Api;

public class ResultsApiModel
{
    public int ResultId { get; set; }

    public int EnrolmentId { get; set; }

    public string? EventName { get; set; }

    public DateTime EventDate { get; set; }

    public string? CategoryName { get; set; }

    public TimeOnly? FinishTime { get; set; }

    public int? FinishPosition { get; set; }
}