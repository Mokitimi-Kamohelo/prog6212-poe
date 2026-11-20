using Microsoft.AspNetCore.Mvc;
using RaceDay.Mvc.Models.ViewModels;
using RaceDay.Mvc.Services;
using System.Net;
using System.Text.Json;
using RaceDay.Mvc.Models.Api;

namespace RaceDay.Mvc.Controllers;

public class AuthController : Controller
{
    private readonly ApiService _apiService;

    public AuthController(ApiService apiService)
    {
        _apiService = apiService;
    }

    // LOGIN

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (IsLoggedIn())
        {
            return RedirectToAction("Index", "Home");
        }

        ViewBag.ReturnUrl = returnUrl;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
        LoginViewModel model,
        string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var request = new
            {
                email = model.Email,
                password = model.Password
            };

            var response = await _apiService.PostResponseAsync(
                "api/auth/login",
                request);

            var responseContent =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    GetApiErrorMessage(responseContent));

                return View(model);
            }

            var authResponse = JsonSerializer.Deserialize<AuthResponseModel>(
                responseContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (authResponse == null ||
                string.IsNullOrWhiteSpace(authResponse.Token))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "The API returned an invalid login response.");

                return View(model);
            }

            // Store authentication information in session
            HttpContext.Session.SetString(
                SessionKeys.JwtToken,
                authResponse.Token);

            HttpContext.Session.SetString(
                SessionKeys.UserId,
                authResponse.UserId.ToString());

            HttpContext.Session.SetString(
                SessionKeys.FirstName,
                authResponse.FirstName ?? string.Empty);

            HttpContext.Session.SetString(
                SessionKeys.LastName,
                authResponse.LastName ?? string.Empty);

            HttpContext.Session.SetString(
                SessionKeys.Email,
                authResponse.Email ?? string.Empty);

            HttpContext.Session.SetString(
                SessionKeys.Role,
                authResponse.Role ?? string.Empty);

            TempData["Success"] =
                $"Welcome back, {authResponse.FirstName}!";

            // Redirect to requested page if local
            if (!string.IsNullOrWhiteSpace(returnUrl) &&
                Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
        catch
        {
            ModelState.AddModelError(
                string.Empty,
                "Unable to connect to the RaceDay API. Please try again.");
        }

        return View(model);
    }

    // REGISTER

    [HttpGet]
    public IActionResult Register()
    {
        if (IsLoggedIn())
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(
        RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            // Match the property names of your API RegisterRequest DTO.
            var request = new
            {
                firstName = model.FirstName,
                lastName = model.LastName,
                email = model.Email,
                password = model.Password,
                phone = model.Phone,
            };

            var response = await _apiService.PostResponseAsync(
                "api/auth/register",
                request);

            var responseContent =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    GetApiErrorMessage(responseContent));

                return View(model);
            }

            TempData["Success"] =
                "Registration successful! You can now log in.";

            return RedirectToAction("Login");
        }
        catch
        {
            ModelState.AddModelError(
                string.Empty,
                "Unable to connect to the RaceDay API. Please try again.");
        }

        return View(model);
    }

    // LOGOUT

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        TempData["Success"] =
            "You have successfully logged out.";

        return RedirectToAction("Index", "Home");
    }

    // HELPERS

    private bool IsLoggedIn()
    {
        return !string.IsNullOrWhiteSpace(
            HttpContext.Session.GetString(
                SessionKeys.JwtToken));
    }

    private string GetApiErrorMessage(string responseContent)
    {
        if (string.IsNullOrWhiteSpace(responseContent))
        {
            return "The request failed. Please check your details.";
        }

        try
        {
            using var document = JsonDocument.Parse(responseContent);

            var root = document.RootElement;

            if (root.TryGetProperty("message", out var message))
            {
                return message.GetString()
                    ?? "The request failed.";
            }

            if (root.TryGetProperty("title", out var title))
            {
                return title.GetString()
                    ?? "The request failed.";
            }

            if (root.TryGetProperty("errors", out var errors))
            {
                return "Please check your submitted information.";
            }
        }
        catch
        {
            // The response may be plain text
        }

        return responseContent.Length > 300
            ? "The request failed. Please check your submitted information."
            : responseContent;
    }
}