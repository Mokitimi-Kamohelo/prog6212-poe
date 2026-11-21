using Microsoft.AspNetCore.Mvc;
using RaceDay.Mvc.Models.Api;
using RaceDay.Mvc.Models.ViewModels;
using RaceDay.Mvc.Services;

namespace RaceDay.Mvc.Controllers;

public class ParticipantController : Controller
{
    private readonly ApiService _apiService;

    public ParticipantController(ApiService apiService)
    {
        _apiService = apiService;
    }

    // MY ENROLMENTS

    [SessionAuthorize("Participant")]
    [HttpGet]
    public async Task<IActionResult> MyEnrolments()
    {
        var model = new MyEnrolmentsViewModel();

        try
        {
            var enrolments =
                await _apiService.GetAsync<List<EnrolmentApiModel>>(
                    "api/Enrolments/my");

            model.Enrolments = enrolments ?? new List<EnrolmentApiModel>();

            model.Enrolments = model.Enrolments
                .OrderByDescending(e => e.EnrolmentDate)
                .ToList();
        }
        catch
        {
            model.ErrorMessage =
                "Unable to load your enrolments. Please try again.";
        }

        return View(model);
    }

    [SessionAuthorize("Participant")]
    [HttpGet]
    public async Task<IActionResult> MyResults()
    {
        try
        {
            var results =
                await _apiService.GetAsync<List<ResultApiModel>>(
                    "api/Results/my");

            return View(results ?? new List<ResultApiModel>());
        }
        catch
        {
            TempData["Error"] =
                "Unable to load your results.";

            return View(new List<ResultApiModel>());
        }
    }

    // CANCEL ENROLMENT

    [SessionAuthorize("Participant")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        if (id <= 0)
        {
            TempData["Error"] =
                "Invalid enrolment.";

            return RedirectToAction(nameof(MyEnrolments));
        }

        try
        {
            var response = await _apiService.PutResponseAsync(
                $"api/Enrolments/{id}/cancel");

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Your enrolment has been cancelled.";
            }
            else
            {
                TempData["Error"] =
                    "Unable to cancel enrolment. " +
                    "It may already have been cancelled.";
            }
        }
        catch
        {
            TempData["Error"] =
                "Unable to connect to the API.";
        }

        return RedirectToAction(nameof(MyEnrolments));
    }
}