using RaceDay.Mvc.Models.Api;
using RaceDay.Mvc.Models.API;
using System.ComponentModel.DataAnnotations;

namespace RaceDay.Mvc.ViewModels;

public class CreateEventViewModel
{
    [Required]
    [StringLength(100)]
    public string EventName { get; set; } = "";

    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = "";

    [Required]
    [DataType(DataType.Date)]
    public DateTime EventDate { get; set; } = DateTime.Today;

    [Required]
    [StringLength(150)]
    public string Location { get; set; } = "";

    [Range(0.01, 100000)]
    public decimal Distance { get; set; }

    [Range(1, int.MaxValue)]
    public int EventTypeId { get; set; }

    public List<EventTypeApiModel> EventTypes { get; set; } = new();
}