using RaceDay.Mvc.Models.API;

namespace RaceDay.Mvc.ViewModels;

public class OrganiserDashboardViewModel
{
    public int TotalEvents { get; set; }
    public int UpcomingEvents { get; set; }
    public int TotalParticipants { get; set; }
    public int CompletedResults { get; set; }

    public List<OrganiserEventApiModel> Events { get; set; } = new();
}