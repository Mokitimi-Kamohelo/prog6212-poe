namespace RaceDay.Mvc.Models.API;

public class OrganiserEventApiModel
{
    public int EventId { get; set; }
    public string EventName { get; set; } = "";
    public string Description { get; set; } = "";
    public DateOnly EventDate { get; set; }
    public string Location { get; set; } = "";
    public decimal Distance { get; set; }
    public int EventTypeId { get; set; }
    public string EventType { get; set; } = "";
    public int CategoryCount { get; set; }
    public int EnrolmentCount { get; set; }
}