using Microsoft.AspNetCore.Mvc;
using RaceDay.Mvc.Models.Api;
using RaceDay.Mvc.Models.API;
using RaceDay.Mvc.Services;
using RaceDay.Mvc.ViewModels;
using System.Net;
using System.Net.Http.Json;

namespace RaceDay.Mvc.Controllers;

[SessionAuthorize("Organiser")]
public class OrganiserController : Controller
{
    private readonly ApiService _api;

    public OrganiserController(ApiService api)
    {
        _api = api;
    }

    // DASHBOARD
    public async Task<IActionResult> Dashboard()
    {
        var events = await _api.GetAsync<List<OrganiserEventApiModel>>(
            "api/Events/my");

        events ??= new List<OrganiserEventApiModel>();

        var upcoming = events
            .Where(e => e.EventDate >= DateOnly.FromDateTime(DateTime.Today))
            .ToList();

        var vm = new OrganiserDashboardViewModel
        {
            TotalEvents = events.Count,
            UpcomingEvents = upcoming.Count,
            Events = events
        };

        // Count enrolments and completed results across managed events.
        foreach (var eventItem in events)
        {
            var participants = await _api.GetAsync<
                List<OrganiserParticipantApiModel>>(
                $"api/Enrolments/event/{eventItem.EventId}");

            if (participants != null)
            {
                vm.TotalParticipants += participants.Count;

                vm.CompletedResults += participants.Count(p =>
                    p.Status == "Completed");
            }
        }

        return View(vm);
    }

    // MY EVENTS
    public async Task<IActionResult> MyEvents()
    {
        var events = await _api.GetAsync<List<OrganiserEventApiModel>>(
            "api/Events/my");

        return View(events ?? new List<OrganiserEventApiModel>());
    }

    // CREATE EVENT - GET
    [HttpGet]
    public async Task<IActionResult> CreateEvent()
    {
        var vm = new CreateEventViewModel();

        await LoadEventTypes(vm);

        return View(vm);
    }

    // CREATE EVENT - POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEvent(
        CreateEventViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            await LoadEventTypes(vm);
            return View(vm);
        }

        var request = new
        {
            vm.EventName,
            vm.Description,
            EventDate = DateOnly.FromDateTime(vm.EventDate),
            vm.Location,
            vm.Distance,
            vm.EventTypeId
        };

        var response = await _api.PostResponseAsync(
            "api/Events",
            request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();

            ModelState.AddModelError(
                "",
                $"Unable to create event: {error}");

            await LoadEventTypes(vm);
            return View(vm);
        }

        TempData["Success"] = "Event created successfully!";

        return RedirectToAction(nameof(MyEvents));
    }

    // MANAGE EVENT
    public async Task<IActionResult> ManageEvent(int id)
    {
        var eventItem = await _api.GetAsync<
            OrganiserEventDetailsApiModel>(
            $"api/Events/{id}");

        if (eventItem == null)
            return NotFound();

        var participants = await _api.GetAsync<
            List<OrganiserParticipantApiModel>>(
            $"api/Enrolments/event/{id}");

        var vm = new ManageEventViewModel
        {
            Event = eventItem,
            Participants = participants ?? new()
        };

        return View(vm);
    }

    // CREATE CATEGORY - GET
    [HttpGet]
    public IActionResult CreateCategory(int eventId)
    {
        return View(new CreateCategoryViewModel
        {
            EventId = eventId
        });
    }

    // CREATE CATEGORY - POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCategory(
        CreateCategoryViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var request = new
        {
            vm.CategoryName,
            vm.CategoryType,
            vm.CategoryValue
        };

        var response = await _api.PostResponseAsync(
            $"api/Categories/event/{vm.EventId}",
            request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();

            ModelState.AddModelError("", error);

            return View(vm);
        }

        TempData["Success"] = "Category created successfully.";

        return RedirectToAction(
            nameof(ManageEvent),
            new { id = vm.EventId });
    }

    // DELETE CATEGORY
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCategory(
        int categoryId,
        int eventId)
    {
        var response = await _api.DeleteResponseAsync(
            $"api/Categories/{categoryId}");

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] =
                await response.Content.ReadAsStringAsync();
        }
        else
        {
            TempData["Success"] = "Category deleted.";
        }

        return RedirectToAction(
            nameof(ManageEvent),
            new { id = eventId });
    }

    // RECORD RESULT - GET
    [HttpGet]
    public async Task<IActionResult> RecordResult(
        int enrolmentId,
        int eventId,
        string participantName,
        string eventName)
    {
        var vm = new CreateResultViewModel
        {
            EnrolmentId = enrolmentId,
            EventId = eventId,
            ParticipantName = participantName,
            EventName = eventName
        };

        return View(vm);
    }

    // RECORD RESULT - POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RecordResult(
        CreateResultViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var request = new
        {
            vm.EnrolmentId,
            FinishTime = TimeOnly.FromTimeSpan(vm.FinishTime),
            vm.FinishPosition
        };

        var response = await _api.PostResponseAsync(
            "api/Results",
            request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();

            ModelState.AddModelError("", error);

            return View(vm);
        }

        TempData["Success"] = "Result recorded successfully.";

        return RedirectToAction(
            nameof(ManageEvent),
            new { id = vm.EventId });
    }

    // EDIT RESULT - GET
    [HttpGet]
    public async Task<IActionResult> EditResult(
        int enrolmentId,
        int eventId)
    {
        var result = await _api.GetAsync<OrganiserResultApiModel>(
            $"api/Results/enrolment/{enrolmentId}");

        if (result == null)
            return NotFound();

        var vm = new CreateResultViewModel
        {
            EnrolmentId = result.EnrolmentId,
            EventId = eventId,
            ParticipantName = result.ParticipantName,
            EventName = result.EventName,
            FinishTime = result.FinishTime.ToTimeSpan(),
            FinishPosition = result.FinishPosition
        };

        ViewBag.ResultId = result.ResultId;

        return View(vm);
    }

    // EDIT RESULT - POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditResult(
        int resultId,
        CreateResultViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ResultId = resultId;
            return View(vm);
        }

        var request = new
        {
            vm.EnrolmentId,
            FinishTime = TimeOnly.FromTimeSpan(vm.FinishTime),
            vm.FinishPosition
        };

        var response = await _api.PutResponseAsync(
            $"api/Results/{resultId}",
            request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();

            ModelState.AddModelError("", error);
            ViewBag.ResultId = resultId;

            return View(vm);
        }

        TempData["Success"] = "Result updated successfully.";

        return RedirectToAction(
            nameof(ManageEvent),
            new { id = vm.EventId });
    }

    // EDIT CATEGORY - GET
    [HttpGet]
    public async Task<IActionResult> EditCategory(int id, int eventId)
    {
        var category = await _api.GetAsync<CategoryApiModel>(
            $"api/Categories/{id}");

        if (category == null)
            return NotFound();

        var vm = new EditCategoryViewModel
        {
            CategoryId = category.CategoryId,
            EventId = eventId,
            CategoryName = category.CategoryName,
            CategoryType = category.CategoryType,
            CategoryValue = category.CategoryValue
        };

        return View(vm);
    }

    // EDIT CATEGORY - POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCategory(
        EditCategoryViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var request = new
        {
            vm.CategoryName,
            vm.CategoryType,
            vm.CategoryValue
        };

        var response = await _api.PutResponseAsync(
            $"api/Categories/{vm.CategoryId}",
            request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();

            ModelState.AddModelError("", error);

            return View(vm);
        }

        TempData["Success"] = "Category updated successfully.";

        return RedirectToAction(
            nameof(ManageEvent),
            new { id = vm.EventId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadBanner(
    int id,
    IFormFile banner)
    {
        if (banner == null || banner.Length == 0)
        {
            TempData["Error"] = "Please select an image.";
            return RedirectToAction("ManageEvent", new { id });
        }

        var response = await _apiService.PutFileResponseAsync(
            $"api/Events/{id}/banner",
            banner);

        if (response.IsSuccessStatusCode)
        {
            TempData["Success"] =
                "Event banner uploaded successfully.";
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();

            TempData["Error"] =
                $"Banner upload failed: {error}";
        }

        return RedirectToAction("ManageEvent", new { id });
    }

    // LOAD EVENT TYPES
    private async Task LoadEventTypes(CreateEventViewModel vm)
    {
        vm.EventTypes = await _api.GetAsync<List<EventTypeApiModel>>(
            "api/EventTypes") ?? new List<EventTypeApiModel>();
    }
}