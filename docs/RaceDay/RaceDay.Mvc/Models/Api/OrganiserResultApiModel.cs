namespace RaceDay.Mvc.Models.API;

public class OrganiserResultApiModel
{
    public int ResultId { get; set; }
    public int EnrolmentId { get; set; }
    public string EventName { get; set; } = "";
    public string ParticipantName { get; set; } = "";
    public TimeOnly FinishTime { get; set; }
    public int FinishPosition { get; set; }
}