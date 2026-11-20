using RaceDay.Mvc.Models.Api;

namespace RaceDay.Mvc.ViewModels;

public class OrganiserDashboardViewModel
{
    public int TotalEvents { get; set; }
    public int UpcomingEvents { get; set; }
    public int TotalParticipants { get; set; }
    public int CompletedResults { get; set; }

    public List<EventApiModel> Events { get; set; } = new();
}