using System.ComponentModel.DataAnnotations;

namespace RaceDay.Api.DTOs;

public class CreateCategoryRequest
{
    [Required]
    [StringLength(100)]
    public string CategoryName { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^(Age|Distance)$",
        ErrorMessage = "CategoryType must be Age or Distance.")]
    public string CategoryType { get; set; } = string.Empty;

    [StringLength(50)]
    public string? CategoryValue { get; set; }
}
