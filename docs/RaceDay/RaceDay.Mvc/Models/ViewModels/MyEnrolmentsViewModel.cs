using RaceDay.Mvc.Models.Api;

namespace RaceDay.Mvc.Models.ViewModels;

public class MyEnrolmentsViewModel
{
    public List<EnrolmentApiModel> Enrolments { get; set; } = new();

    public string? ErrorMessage { get; set; }
}