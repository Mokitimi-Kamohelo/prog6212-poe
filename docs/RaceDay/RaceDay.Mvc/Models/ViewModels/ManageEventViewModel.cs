using RaceDay.Mvc.Models.Api;

namespace RaceDay.Mvc.ViewModels;

public class ManageEventViewModel
{
    public EventApiModel Event { get; set; } = new();

    public List<CategoryApiModel> Categories { get; set; } = new();

    public List<EnrolmentApiModel> Participants { get; set; } = new();
}