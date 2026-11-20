namespace RaceDay.Mvc.Models.API;

public class OrganiserParticipantApiModel
{
    public int EnrolmentId { get; set; }

    public int ParticipantId { get; set; }

    public string ParticipantName { get; set; } = "";

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = "";

    public DateTime EnrolmentDate { get; set; }

    public string Status { get; set; } = "";
}