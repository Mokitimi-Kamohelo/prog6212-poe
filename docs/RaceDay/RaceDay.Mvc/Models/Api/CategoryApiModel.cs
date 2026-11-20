namespace RaceDay.Mvc.Models.Api;

public class CategoryApiModel
{
    public int CategoryId { get; set; }

    public int EventId { get; set; }

    public string? CategoryName { get; set; }

    public string? CategoryType { get; set; }

    public string? CategoryValue { get; set; }
}