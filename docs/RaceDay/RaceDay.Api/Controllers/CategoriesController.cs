using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.DTOs;
using RaceDay.Api.Models;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly RaceDayContext _context;

    public CategoriesController(RaceDayContext context)
    {
        _context = context;
    }

    // GET: api/Categories
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _context.Categories
            .AsNoTracking()
            .Select(c => new
            {
                c.CategoryId,
                c.EventId,
                EventName = c.Event.EventName,
                c.CategoryName,
                c.CategoryType,
                c.CategoryValue
            })
            .ToListAsync();

        return Ok(categories);
    }

    // GET: api/Categories/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        var category = await _context.Categories
            .AsNoTracking()
            .Where(c => c.CategoryId == id)
            .Select(c => new
            {
                c.CategoryId,
                c.EventId,
                EventName = c.Event.EventName,
                c.CategoryName,
                c.CategoryType,
                c.CategoryValue
            })
            .FirstOrDefaultAsync();

        if (category == null)
        {
            return NotFound(new
            {
                message = "Category not found."
            });
        }

        return Ok(category);
    }

    // GET: api/Categories/event/5
    [HttpGet("event/{eventId:int}")]
    public async Task<IActionResult> GetCategoriesByEvent(int eventId)
    {
        var eventExists = await _context.Events
            .AnyAsync(e => e.EventId == eventId);

        if (!eventExists)
        {
            return NotFound(new
            {
                message = "Event not found."
            });
        }

        var categories = await _context.Categories
            .AsNoTracking()
            .Where(c => c.EventId == eventId)
            .Select(c => new
            {
                c.CategoryId,
                c.EventId,
                c.CategoryName,
                c.CategoryType,
                c.CategoryValue
            })
            .ToListAsync();

        return Ok(categories);
    }

    // POST: api/Categories/event/5
    [HttpPost("event/{eventId:int}")]
    public async Task<IActionResult> CreateCategory(
        int eventId,
        [FromBody] CreateCategoryRequest request)
    {
        var eventItem = await _context.Events
            .FirstOrDefaultAsync(e => e.EventId == eventId);

        if (eventItem == null)
        {
            return NotFound(new
            {
                message = "Event not found."
            });
        }

        var category = new Category
        {
            EventId = eventId,
            CategoryName = request.CategoryName,
            CategoryType = request.CategoryType,
            CategoryValue = request.CategoryValue
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetCategory),
            new { id = category.CategoryId },
            new
            {
                category.CategoryId,
                category.EventId,
                category.CategoryName,
                category.CategoryType,
                category.CategoryValue
            });
    }

    // PUT: api/Categories/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCategory(
        int id,
        [FromBody] UpdateCategoryRequest request)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.CategoryId == id);

        if (category == null)
        {
            return NotFound(new
            {
                message = "Category not found."
            });
        }

        category.CategoryName = request.CategoryName;
        category.CategoryType = request.CategoryType;
        category.CategoryValue = request.CategoryValue;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Categories/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.CategoryId == id);

        if (category == null)
        {
            return NotFound(new
            {
                message = "Category not found."
            });
        }

        // Do not delete a category that is referenced by enrolments.
        var hasEnrolments = await _context.Enrolments
            .AnyAsync(e => e.CategoryId == id);

        if (hasEnrolments)
        {
            return Conflict(new
            {
                message = "This category has enrolments and cannot be deleted."
            });
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
