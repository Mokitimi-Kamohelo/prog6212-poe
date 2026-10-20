using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            return NotFound(new { message = "Event not found." });

        return Ok(eventItem);
    }

    // POST: api/Events
    // Temporary: authentication and organiser ownership checks
    // will be added when JWT authentication is configured.
    [HttpPost]
    public async Task<IActionResult> CreateEvent(
        [FromBody] CreateEventRequest request)
    {
        if (!await _context.EventTypes
            .AnyAsync(t => t.EventTypeId == request.EventTypeId))
        {
            return BadRequest(new
            {
                message = "The selected event type does not exist."
            });
        }

        // Temporary development value only.
        // Replace this with the authenticated organiser's ID
        // before treating the endpoint as production-ready.
        var organiserId = await _context.Users
            .Where(u => u.Role.RoleName == "Organiser")
            .Select(u => u.UserId)
            .FirstOrDefaultAsync();

        if (organiserId == 0)
        {
            return BadRequest(new
            {
                message = "No organiser exists in the database."
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
