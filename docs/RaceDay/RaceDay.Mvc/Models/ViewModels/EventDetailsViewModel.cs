using RaceDay.Mvc.Models.Api;

namespace RaceDay.Mvc.Models.ViewModels;

public class EventDetailsViewModel
{
    public EventApiModel Event { get; set; } = new();

    public bool IsLoggedIn { get; set; }

    public bool IsParticipant { get; set; }

    public bool AlreadyEnrolled { get; set; }

    public int? ExistingEnrolmentId { get; set; }

    public string? ErrorMessage { get; set; }
}