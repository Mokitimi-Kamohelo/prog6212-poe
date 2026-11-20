using Microsoft.AspNetCore.Mvc;
using RaceDay.Mvc.Models.ViewModels;
using RaceDay.Mvc.Services;

namespace RaceDay.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ApiService _apiService;

    public HomeController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        var model = new ApiStatusViewModel();

        try
        {
            var response = await _apiService.GetAsync<object>(
                "api/EventTypes");

            model.ApiConnected = true;
            model.Message = "RaceDay API is connected successfully!";
            model.ApiResponse = response?.ToString();
        }
        catch (Exception ex)
        {
            model.ApiConnected = false;
            model.Message = "Unable to connect to the RaceDay API.";
            model.ApiResponse = ex.Message;
        }

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }
}