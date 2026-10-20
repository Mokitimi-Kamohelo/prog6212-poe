using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using RaceDay.Api.Models;
using RaceDay.Api.DTOs;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly RaceDayContext _context;

    public EventsController(RaceDayContext context)
    {
        _context = context;
    }

    // GET: api/Events
    // Public: anyone can browse events.
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetEvents()
    {
        var events = await _context.Events
            .AsNoTracking()
            .Select(e => new
            {
                e.EventId,
                e.EventName,
                e.Description,
                e.EventDate,
                e.Location,
                e.Distance,
                e.EventTypeId,
                EventType = e.EventType.TypeName,
                Organiser = e.Organiser.FirstName + " " +
                            e.Organiser.LastName,
                CategoryCount = e.Categories.Count
            })
            .ToListAsync();

        return Ok(events);
    }

    // GET: api/Events/5
    // Public: anyone can view an event and its categories.
    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetEvent(int id)
    {
        var eventItem = await _context.Events
            .AsNoTracking()
            .Where(e => e.EventId == id)
            .Select(e => new
            {
                e.EventId,
                e.EventName,
                e.Description,
                e.EventDate,
                e.Location,
                e.Distance,
                e.EventTypeId,
                EventType = e.EventType.TypeName,
                Organiser = e.Organiser.FirstName + " " +
                            e.Organiser.LastName,
                Categories = e.Categories.Select(c => new
                {
                    c.CategoryId,
                    c.CategoryName,
                    c.CategoryType,
                    c.CategoryValue
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (eventItem == null)
        {
            return NotFound(new
            {
                message = "Event not found."
            });
        }

        return Ok(eventItem);
    }

    // POST: api/Events
    // Only authenticated Organisers can create events.
    [Authorize(Roles = "Organiser")]
    [HttpPost]
    public async Task<IActionResult> CreateEvent(
        [FromBody] CreateEventRequest request)
    {
        var userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out var organiserId))
        {
            return Unauthorized(new
            {
                message = "The authenticated user's ID is missing or invalid."
            });
        }

        var organiserExists = await _context.Users
            .AnyAsync(u =>
                u.UserId == organiserId &&
                u.Role.RoleName == "Organiser");

        if (!organiserExists)
        {
            return Forbid();
        }

        var eventTypeExists = await _context.EventTypes
            .AnyAsync(t => t.EventTypeId == request.EventTypeId);

        if (!eventTypeExists)
        {
            return BadRequest(new
            {
                message = "The selected event type does not exist."
            });
        }

        var newEvent = new Event
        {
            OrganiserId = organiserId,
            EventTypeId = request.EventTypeId,
            EventName = request.EventName,
            Description = request.Description,
            EventDate = request.EventDate,
            Location = request.Location,
            Distance = request.Distance
        };

        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetEvent),
            new { id = newEvent.EventId },
            new
            {
                newEvent.EventId,
                newEvent.EventName,
                newEvent.EventDate,
                newEvent.Location
            });
    }
}
