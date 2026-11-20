namespace RaceDay.Mvc.Models.ViewModels;

public class ApiStatusViewModel
{
    public bool ApiConnected { get; set; }

    public string Message { get; set; } = string.Empty;

    public string? ApiResponse { get; set; }
}