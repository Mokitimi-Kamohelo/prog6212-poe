using Microsoft.AspNetCore.Mvc;
using RaceDay.Mvc.Models.Api;
using RaceDay.Mvc.Models.ViewModels;
using RaceDay.Mvc.Services;
using System.Text.Json;

namespace RaceDay.Mvc.Controllers;

public class EventsController : Controller
{
    private readonly ApiService _apiService;

    public EventsController(ApiService apiService)
    {
        _apiService = apiService;
    }

    // EVENT LIST

    [HttpGet]
    public async Task<IActionResult> Index(
        string? search,
        int? eventTypeId,
        DateTime? date)
    {
        var model = new EventListViewModel
        {
            SearchTerm = search,
            SelectedEventTypeId = eventTypeId,
            SelectedDate = date
        };

        try
        {
            var events = await _apiService.GetAsync<List<EventApiModel>>(
                "api/Events");

            var eventTypes = await _apiService.GetAsync<List<EventTypeApiModel>>(
                "api/EventTypes");

            model.Events = events ?? new List<EventApiModel>();

            model.EventTypes = eventTypes ?? new List<EventTypeApiModel>();

            // Only display upcoming events
            model.Events = model.Events
                .Where(e => e.EventDate.Date >= DateTime.Today)
                .ToList();

            // Search by name or location
            if (!string.IsNullOrWhiteSpace(search))
            {
                model.Events = model.Events
                    .Where(e =>
                        (e.EventName ?? "").Contains(
                            search,
                            StringComparison.OrdinalIgnoreCase)
                        ||
                        (e.Location ?? "").Contains(
                            search,
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Filter by event type
            if (eventTypeId.HasValue)
            {
                model.Events = model.Events
                    .Where(e => e.EventTypeId == eventTypeId.Value)
                    .ToList();
            }

            // Filter by date
            if (date.HasValue)
            {
                model.Events = model.Events
                    .Where(e => e.EventDate.Date == date.Value.Date)
                    .ToList();
            }

            model.Events = model.Events
                .OrderBy(e => e.EventDate)
                .ToList();
        }
        catch
        {
            model.ErrorMessage =
                "Unable to load events. Please ensure the RaceDay API is running.";
        }

        return View(model);
    }

    // EVENT DETAILS

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }

        var model = new EventDetailsViewModel();

        try
        {
            var eventDetails = await _apiService.GetAsync<EventApiModel>(
                $"api/Events/{id}");

            if (eventDetails == null)
            {
                return NotFound();
            }

            model.Event = eventDetails;

            model.IsLoggedIn =
                !string.IsNullOrWhiteSpace(
                    HttpContext.Session.GetString(SessionKeys.JwtToken));

            model.IsParticipant =
                string.Equals(
                    HttpContext.Session.GetString(SessionKeys.Role),
                    "Participant",
                    StringComparison.OrdinalIgnoreCase);

            // Load categories separately if not included in event response
            if (model.Event.Categories == null ||
                model.Event.Categories.Count == 0)
            {
                var categories =
                    await _apiService.GetAsync<List<CategoryApiModel>>(
                        $"api/Categories/event/{id}");

                model.Event.Categories =
                    categories ?? new List<CategoryApiModel>();
            }

            // Check if this Participant has already enrolled
            if (model.IsLoggedIn && model.IsParticipant)
            {
                var enrolments =
                    await _apiService.GetAsync<List<EnrolmentApiModel>>(
                        "api/Enrolments/my");

                var existing = enrolments?.FirstOrDefault(e =>
                    e.EventId == id &&
                    !string.Equals(
                        e.Status,
                        "Cancelled",
                        StringComparison.OrdinalIgnoreCase));

                if (existing != null)
                {
                    model.AlreadyEnrolled = true;
                    model.ExistingEnrolmentId =
                        existing.EnrolmentId;
                }
            }
        }
        catch
        {
            model.ErrorMessage =
                "Unable to load event details.";
        }

        return View(model);
    }

    // ENROL

    [HttpPost]
    [ValidateAntiForgeryToken]
    [SessionAuthorize("Participant")]
    public async Task<IActionResult> Enrol(
        int eventId,
        int categoryId)
    {
        if (eventId <= 0 || categoryId <= 0)
        {
            TempData["Error"] =
                "Please select a valid event and category.";

            return RedirectToAction(
                "Details",
                new { id = eventId });
        }

        try
        {
            var request = new
            {
                eventId = eventId,
                categoryId = categoryId
            };

            var response = await _apiService.PostResponseAsync(
                "api/Enrolments",
                request);

            var responseContent =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] =
                    GetApiErrorMessage(responseContent);

                return RedirectToAction(
                    "Details",
                    new { id = eventId });
            }

            TempData["Success"] =
                "You have successfully enrolled in this event!";

            return RedirectToAction(
                "MyEnrolments",
                "Participant");
        }
        catch
        {
            TempData["Error"] =
                "Unable to enrol. Please try again.";
        }

        return RedirectToAction(
            "Details",
            new { id = eventId });
    }

    // ERROR HELPER

    private string GetApiErrorMessage(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return "The request failed. Please try again.";
        }

        try
        {
            using var document = JsonDocument.Parse(content);

            var root = document.RootElement;

            if (root.TryGetProperty("message", out var message))
            {
                return message.GetString()
                    ?? "The request failed.";
            }

            if (root.TryGetProperty("title", out var title))
            {
                return title.GetString()
                    ?? "The request failed.";
            }
        }
        catch
        {
            // Response was not JSON
        }

        return content.Length > 250
            ? "Unable to complete the request."
            : content;
    }
}