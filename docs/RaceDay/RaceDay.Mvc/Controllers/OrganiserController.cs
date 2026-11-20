using Microsoft.AspNetCore.Mvc;
using RaceDay.Mvc.Services;

[SessionAuthorize("Organiser")]
public class OrganiserController : Controller
{
    // API service injected here
}