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

    // GET: api/Events/my
    [Authorize(Roles = "Organiser")]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyEvents()
    {
        var userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out var organiserId))
            return Unauthorized();

        var events = await _context.Events
            .AsNoTracking()
            .Where(e => e.OrganiserId == organiserId)
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
                CategoryCount = e.Categories.Count,
                EnrolmentCount = e.Enrolments.Count
            })
            .OrderByDescending(e => e.EventDate)
            .ToListAsync();

        return Ok(events);
    }

    // PUT: api/Events/5
    [Authorize(Roles = "Organiser")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateEvent(
        int id,
        [FromBody] UpdateEventRequest request)
    {
        var userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out var organiserId))
            return Unauthorized();

        var eventItem = await _context.Events
            .FirstOrDefaultAsync(e =>
                e.EventId == id &&
                e.OrganiserId == organiserId);

        if (eventItem == null)
            return NotFound(new
            {
                message = "Event not found or you do not manage this event."
            });

        var eventTypeExists = await _context.EventTypes
            .AnyAsync(t => t.EventTypeId == request.EventTypeId);

        if (!eventTypeExists)
            return BadRequest(new
            {
                message = "The selected event type does not exist."
            });

        eventItem.EventName = request.EventName;
        eventItem.Description = request.Description;
        eventItem.EventDate = request.EventDate;
        eventItem.Location = request.Location;
        eventItem.Distance = request.Distance;
        eventItem.EventTypeId = request.EventTypeId;

        await _context.SaveChangesAsync();

        return NoContent();
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

    // DELETE: api/Events/5
    [Authorize(Roles = "Organiser")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out var organiserId))
            return Unauthorized();

        var eventItem = await _context.Events
            .FirstOrDefaultAsync(e =>
                e.EventId == id &&
                e.OrganiserId == organiserId);

        if (eventItem == null)
            return NotFound(new
            {
                message = "Event not found or you do not manage this event."
            });

        var hasEnrolments = await _context.Enrolments
            .AnyAsync(e => e.EventId == id);

        if (hasEnrolments)
            return Conflict(new
            {
                message = "Events with enrolments cannot be deleted."
            });

        _context.Events.Remove(eventItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
