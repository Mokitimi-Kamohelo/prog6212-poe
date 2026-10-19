using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthTestController : ControllerBase
{
    [Authorize]
    [HttpGet("protected")]
    public IActionResult Protected()
    {
        return Ok(new
        {
            message = "You are authenticated!",
            userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier),
            email = User.FindFirstValue(ClaimTypes.Email),
            role = User.FindFirstValue(ClaimTypes.Role)
        });
    }

    [Authorize(Roles = "Participant")]
    [HttpGet("participant")]
    public IActionResult ParticipantOnly()
    {
        return Ok(new
        {
            message = "Participant access granted."
        });
    }
}
