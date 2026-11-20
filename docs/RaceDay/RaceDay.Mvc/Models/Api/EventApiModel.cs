namespace RaceDay.Mvc.Models.Api;

public class EventApiModel
{
    public int EventId { get; set; }

    public string EventName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime EventDate { get; set; }

    public string? Location { get; set; }

    public decimal Distance { get; set; }

    public int EventTypeId { get; set; }

    public string? EventType { get; set; }

    public string? Organiser { get; set; }

    public int CategoryCount { get; set; }

    public List<CategoryApiModel> Categories { get; set; } = new();
}