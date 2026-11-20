namespace RaceDay.Api.Configuration;

public class AzureStorageSettings
{
    public string ConnectionString { get; set; } = string.Empty;

    public string EventBannersContainer { get; set; } = "event-banners";

    public string ProfilePicturesContainer { get; set; } = "profile-pictures";
}
