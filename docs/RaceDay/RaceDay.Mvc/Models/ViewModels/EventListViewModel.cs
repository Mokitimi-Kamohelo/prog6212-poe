using RaceDay.Mvc.Models.Api;

namespace RaceDay.Mvc.Models.ViewModels;

public class EventListViewModel
{
    public List<EventApiModel> Events { get; set; } = new();

    public List<EventTypeApiModel> EventTypes { get; set; } = new();

    public string? SearchTerm { get; set; }

    public int? SelectedEventTypeId { get; set; }

    public DateTime? SelectedDate { get; set; }

    public string? ErrorMessage { get; set; }
}