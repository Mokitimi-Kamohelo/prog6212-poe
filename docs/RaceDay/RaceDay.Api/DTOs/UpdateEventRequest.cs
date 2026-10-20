using System.ComponentModel.DataAnnotations;

namespace RaceDay.Api.DTOs;

public class UpdateEventRequest
{
    [Required]
    [StringLength(100)]
    public string EventName { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateOnly EventDate { get; set; }

    [Required]
    [StringLength(150)]
    public string Location { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "100000")]
    public decimal Distance { get; set; }

    [Range(1, int.MaxValue)]
    public int EventTypeId { get; set; }
}
