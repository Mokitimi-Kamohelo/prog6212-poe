using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.DTOs;
using RaceDay.Api.Models;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResultsController : ControllerBase
{
    private readonly RaceDayContext _context;

    public ResultsController(RaceDayContext context)
    {
        _context = context;
    }

    private int? GetCurrentUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(value, out var id) ? id : null;
    }

    // GET: api/Results/my
    [Authorize(Roles = "Participant")]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyResults()
    {
        var participantId = GetCurrentUserId();

        if (participantId == null)
            return Unauthorized();

        var results = await _context.Results
            .AsNoTracking()
            .Where(r => r.Enrolment.ParticipantId == participantId.Value)
            .Select(r => new
            {
                r.ResultId,
                r.EnrolmentId,
                EventName = r.Enrolment.Event.EventName,
                EventDate = r.Enrolment.Event.EventDate,
                CategoryName = r.Enrolment.Category.CategoryName,
                r.FinishTime,
                r.FinishPosition
            })
            .ToListAsync();

        return Ok(results);
    }

    // GET: api/Results/enrolment/5
    [Authorize(Roles = "Organiser")]
    [HttpGet("enrolment/{enrolmentId:int}")]
    public async Task<IActionResult> GetResultByEnrolment(int enrolmentId)
    {
        var organiserId = GetCurrentUserId();

        if (organiserId == null)
            return Unauthorized();

        var result = await _context.Results
            .AsNoTracking()
            .Where(r =>
                r.EnrolmentId == enrolmentId &&
                r.Enrolment.Event.OrganiserId == organiserId.Value)
            .Select(r => new
            {
                r.ResultId,
                r.EnrolmentId,
                EventName = r.Enrolment.Event.EventName,
                ParticipantName = r.Enrolment.Participant.FirstName + " " +
                                  r.Enrolment.Participant.LastName,
                r.FinishTime,
                r.FinishPosition
            })
            .FirstOrDefaultAsync();

        if (result == null)
            return NotFound(new
            {
                message = "Result not found for an event you manage."
            });

        return Ok(result);
    }

    // POST: api/Results
    [Authorize(Roles = "Organiser")]
    [HttpPost]
    public async Task<IActionResult> CreateResult(
        [FromBody] CreateResultRequest request)
    {
        var organiserId = GetCurrentUserId();

        if (organiserId == null)
            return Unauthorized();

        var enrolment = await _context.Enrolments
            .Include(e => e.Event)
            .FirstOrDefaultAsync(e =>
                e.EnrolmentId == request.EnrolmentId &&
                e.Event.OrganiserId == organiserId.Value);

        if (enrolment == null)
        {
            return NotFound(new
            {
                message = "Enrolment not found for an event you manage."
            });
        }

        if (enrolment.Status == "Cancelled")
        {
            return BadRequest(new
            {
                message = "A cancelled enrolment cannot receive a result."
            });
        }

        var alreadyHasResult = await _context.Results
            .AnyAsync(r => r.EnrolmentId == request.EnrolmentId);

        if (alreadyHasResult)
        {
            return Conflict(new
            {
                message = "This enrolment already has a result."
            });
        }

        var result = new Result
        {
            EnrolmentId = request.EnrolmentId,
            FinishTime = request.FinishTime,
            FinishPosition = request.FinishPosition
        };

        _context.Results.Add(result);
        enrolment.Status = "Completed";

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict(new
            {
                message = "The result could not be saved. Check whether one already exists."
            });
        }

        return CreatedAtAction(
            nameof(GetResultByEnrolment),
            new { enrolmentId = result.EnrolmentId },
            new
            {
                result.ResultId,
                result.EnrolmentId,
                result.FinishTime,
                result.FinishPosition
            });
    }

    // PUT: api/Results/5
    [Authorize(Roles = "Organiser")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateResult(
        int id,
        [FromBody] CreateResultRequest request)
    {
        var organiserId = GetCurrentUserId();

        if (organiserId == null)
            return Unauthorized();

        var result = await _context.Results
            .Include(r => r.Enrolment)
                .ThenInclude(e => e.Event)
            .FirstOrDefaultAsync(r =>
                r.ResultId == id &&
                r.Enrolment.Event.OrganiserId == organiserId.Value);

        if (result == null)
        {
            return NotFound(new
            {
                message = "Result not found for an event you manage."
            });
        }

        if (result.EnrolmentId != request.EnrolmentId)
        {
            return BadRequest(new
            {
                message = "A result cannot be reassigned to another enrolment."
            });
        }

        result.FinishTime = request.FinishTime;
        result.FinishPosition = request.FinishPosition;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
