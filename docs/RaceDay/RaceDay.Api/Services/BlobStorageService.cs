using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace RaceDay.Api.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    private const long MaxFileSize = 5 * 1024 * 1024;

    private static readonly string[] AllowedContentTypes =
    {
        "image/jpeg",
        "image/png",
        "image/webp"
    };

    public BlobStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string> UploadImageAsync(
        IFormFile file,
        string containerName)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Please select an image.");

        if (file.Length > MaxFileSize)
            throw new ArgumentException("Image must not exceed 5 MB.");

        if (!AllowedContentTypes.Contains(
            file.ContentType.ToLowerInvariant()))
        {
            throw new ArgumentException(
                "Only JPG, PNG and WEBP images are allowed.");
        }

        var extension = file.ContentType.ToLowerInvariant() switch
        {
            "image/jpeg" => ".jpg",
            "image/png" => ".png",
            "image/webp" => ".webp",
            _ => throw new ArgumentException("Invalid image type.")
        };

        var blobName = $"{Guid.NewGuid()}{extension}";

        var containerClient =
            _blobServiceClient.GetBlobContainerClient(containerName);

        await containerClient.CreateIfNotExistsAsync(
            PublicAccessType.Blob);

        var blobClient = containerClient.GetBlobClient(blobName);

        using var stream = file.OpenReadStream();

        await blobClient.UploadAsync(
            stream,
            new BlobHttpHeaders
            {
                ContentType = file.ContentType
            });

        return blobClient.Uri.ToString();
    }

    public async Task DeleteImageAsync(
        string imageUrl,
        string containerName)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            return;

        var containerClient =
            _blobServiceClient.GetBlobContainerClient(containerName);

        var blobName = Path.GetFileName(
            new Uri(imageUrl).AbsolutePath);

        var blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.DeleteIfExistsAsync();
    }
}
