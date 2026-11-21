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
            var response = await _apiService.GetResponseAsync("api/Events");

            model.ApiConnected = response.IsSuccessStatusCode;

            if (!model.ApiConnected)
            {
                model.Message = "The RaceDay API returned an unsuccessful response.";
            }
        }
        catch (Exception)
        {
            model.ApiConnected = false;
            model.Message = "Unable to connect to the RaceDay API.";
        }

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
