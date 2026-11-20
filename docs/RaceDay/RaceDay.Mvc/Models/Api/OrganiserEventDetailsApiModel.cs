using RaceDay.Mvc.Models.Api;

namespace RaceDay.Mvc.Models.API;

public class OrganiserEventDetailsApiModel
{
    public int EventId { get; set; }
    public string EventName { get; set; } = "";
    public string Description { get; set; } = "";
    public DateOnly EventDate { get; set; }
    public string Location { get; set; } = "";
    public decimal Distance { get; set; }
    public int EventTypeId { get; set; }
    public string EventType { get; set; } = "";
    public string Organiser { get; set; } = "";
    public string? BannerUrl { get; set; }

    public List<CategoryApiModel> Categories { get; set; } = new();
}