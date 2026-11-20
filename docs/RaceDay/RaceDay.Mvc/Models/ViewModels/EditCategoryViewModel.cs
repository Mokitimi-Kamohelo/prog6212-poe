using System.ComponentModel.DataAnnotations;

namespace RaceDay.Mvc.ViewModels;

public class EditCategoryViewModel
{
    public int CategoryId { get; set; }

    public int EventId { get; set; }

    [Required]
    [StringLength(100)]
    public string CategoryName { get; set; } = "";

    [Required]
    public string CategoryType { get; set; } = "Age";

    [StringLength(50)]
    public string? CategoryValue { get; set; }
}