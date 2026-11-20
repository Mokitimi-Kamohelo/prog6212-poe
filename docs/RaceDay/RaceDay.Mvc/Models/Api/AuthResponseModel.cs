namespace RaceDay.Mvc.Models.Api;

public class AuthResponseModel
{
    public string? Token { get; set; }

    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Role { get; set; }

    public DateTime? ExpiresAt { get; set; }
}