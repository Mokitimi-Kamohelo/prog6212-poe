namespace RaceDay.Mvc.Models.Api;

public class EnrolmentApiModel
{
    public int EnrolmentId { get; set; }

    public int EventId { get; set; }

    public string? EventName { get; set; }

    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public DateTime EnrolmentDate { get; set; }

    public string? Status { get; set; }

    public ResultApiModel? Result { get; set; }
}