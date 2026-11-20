using RaceDay.Mvc.Models.API;

namespace RaceDay.Mvc.ViewModels;

public class ManageEventViewModel
{
    public OrganiserEventDetailsApiModel Event { get; set; } = new();

    public List<OrganiserParticipantApiModel> Participants { get; set; } = new();
}