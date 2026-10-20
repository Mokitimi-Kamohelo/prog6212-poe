using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.DTOs;
using RaceDay.Api.Models;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrolmentsController : ControllerBase
{
    private readonly RaceDayContext _context;

    public EnrolmentsController(RaceDayContext context)
    {
        _context = context;
    }

    private int? GetCurrentUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return int.TryParse(value, out var id) ? id : null;
    }

    // GET: api/Enrolments/my
    [Authorize(Roles = "Participant")]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyEnrolments()
    {
        var participantId = GetCurrentUserId();

        if (participantId == null)
            return Unauthorized();

        var enrolments = await _context.Enrolments
            .AsNoTracking()
            .Where(e => e.ParticipantId == participantId.Value)
            .Select(e => new
            {
                e.EnrolmentId,
                e.EventId,
                EventName = e.Event.EventName,
                e.CategoryId,
                CategoryName = e.Category.CategoryName,
                e.EnrolmentDate,
                e.Status,
                Result = e.Result == null ? null : new
                {
                    e.Result.FinishTime,
                    e.Result.FinishPosition
                }
            })
            .ToListAsync();

        return Ok(enrolments);
    }

    // POST: api/Enrolments
    [Authorize(Roles = "Participant")]
    [HttpPost]
    public async Task<IActionResult> CreateEnrolment(
        [FromBody] CreateEnrolmentRequest request)
    {
        var participantId = GetCurrentUserId();

        if (participantId == null)
            return Unauthorized();

        var eventItem = await _context.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EventId == request.EventId);

        if (eventItem == null)
            return NotFound(new { message = "Event not found." });

        var category = await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c =>
                c.CategoryId == request.CategoryId &&
                c.EventId == request.EventId);

        if (category == null)
        {
            return BadRequest(new
            {
                message = "The selected category does not belong to this event."
            });
        }

        var alreadyEnrolled = await _context.Enrolments
            .AnyAsync(e =>
                e.ParticipantId == participantId.Value &&
                e.EventId == request.EventId);

        if (alreadyEnrolled)
        {
            return Conflict(new
            {
                message = "You are already enrolled in this event."
            });
        }

        var enrolment = new Enrolment
        {
            ParticipantId = participantId.Value,
            EventId = request.EventId,
            CategoryId = request.CategoryId,
            Status = "Registered"
        };

        _context.Enrolments.Add(enrolment);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // The database's unique constraint is the final
            // protection against duplicate enrolments.
            return Conflict(new
            {
                message = "The enrolment could not be saved. Check for an existing enrolment."
            });
        }

        return CreatedAtAction(
            nameof(GetMyEnrolments),
            new { },
            new
            {
                enrolment.EnrolmentId,
                enrolment.EventId,
                enrolment.CategoryId,
                enrolment.EnrolmentDate,
                enrolment.Status
            });
    }

    // PUT: api/Enrolments/5/cancel
    [Authorize(Roles = "Participant")]
    [HttpPut("{id:int}/cancel")]
    public async Task<IActionResult> CancelEnrolment(int id)
    {
        var participantId = GetCurrentUserId();

        if (participantId == null)
            return Unauthorized();

        var enrolment = await _context.Enrolments
            .FirstOrDefaultAsync(e =>
                e.EnrolmentId == id &&
                e.ParticipantId == participantId.Value);

        if (enrolment == null)
        {
            return NotFound(new
            {
                message = "Enrolment not found for this participant."
            });
        }

        if (enrolment.Status != "Registered")
        {
            return Conflict(new
            {
                message = "Only registered enrolments can be cancelled."
            });
        }

        enrolment.Status = "Cancelled";
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/Enrolments/event/5
    [Authorize(Roles = "Organiser")]
    [HttpGet("event/{eventId:int}")]
    public async Task<IActionResult> GetEventEnrolments(int eventId)
    {
        var organiserId = GetCurrentUserId();

        if (organiserId == null)
            return Unauthorized();

        var eventExists = await _context.Events
            .AnyAsync(e =>
                e.EventId == eventId &&
                e.OrganiserId == organiserId.Value);

        if (!eventExists)
        {
            return NotFound(new
            {
                message = "Event not found or you do not manage this event."
            });
        }

        var enrolments = await _context.Enrolments
            .AsNoTracking()
            .Where(e => e.EventId == eventId)
            .Select(e => new
            {
                e.EnrolmentId,
                e.ParticipantId,
                ParticipantName = e.Participant.FirstName + " " +
                                  e.Participant.LastName,
                e.CategoryId,
                CategoryName = e.Category.CategoryName,
                e.EnrolmentDate,
                e.Status
            })
            .ToListAsync();

        return Ok(enrolments);
    }
}
