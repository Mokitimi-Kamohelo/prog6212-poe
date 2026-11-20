using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;


namespace RaceDay.Mvc.Services;

public class ApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiService(
        IHttpClientFactory httpClientFactory,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    private HttpClient CreateClient()
    {
        var client = _httpClientFactory.CreateClient("RaceDayApi");

        var token = _httpContextAccessor
            .HttpContext?
            .Session
            .GetString(SessionKeys.JwtToken);

        if (!string.IsNullOrWhiteSpace(token))
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        return client;
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        var client = CreateClient();

        var response = await client.GetAsync(endpoint);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(
        string endpoint,
        TRequest data)
    {
        var client = CreateClient();

        var response = await client.PostAsJsonAsync(endpoint, data);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task PostAsync<TRequest>(
        string endpoint,
        TRequest data)
    {
        var client = CreateClient();

        var response = await client.PostAsJsonAsync(endpoint, data);

        response.EnsureSuccessStatusCode();
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(
        string endpoint,
        TRequest data)
    {
        var client = CreateClient();

        var response = await client.PutAsJsonAsync(endpoint, data);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task PutAsync<TRequest>(
        string endpoint,
        TRequest data)
    {
        var client = CreateClient();

        var response = await client.PutAsJsonAsync(endpoint, data);

        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string endpoint)
    {
        var client = CreateClient();

        var response = await client.DeleteAsync(endpoint);

        response.EnsureSuccessStatusCode();
    }

    public async Task<HttpResponseMessage> PostResponseAsync<TRequest>(
    string endpoint,
    TRequest data)
    {
        var client = CreateClient();

        return await client.PostAsJsonAsync(endpoint, data);
    }

    public async Task<HttpResponseMessage> GetResponseAsync(
    string endpoint)
    {
        var client = CreateClient();

        return await client.GetAsync(endpoint);
    }

    public async Task<HttpResponseMessage> PutResponseAsync<TRequest>(
    string endpoint,
    TRequest data)
    {
        var client = CreateClient();

        return await client.PutAsJsonAsync(endpoint, data);
    }

    public async Task<HttpResponseMessage> PutResponseAsync(
    string endpoint)
    {
        var client = CreateClient();

        return await client.PutAsync(endpoint, null);
    }

    public async Task<HttpResponseMessage> DeleteResponseAsync(
    string endpoint)
    {
        var client = CreateClient();

        return await client.DeleteAsync(endpoint);
    }

    public async Task<HttpResponseMessage> PutFileResponseAsync(
    string endpoint,
    IFormFile file)
    {
        var client = CreateClient();

        using var content = new MultipartFormDataContent();

        using var stream = file.OpenReadStream();

        var fileContent = new StreamContent(stream);

        fileContent.Headers.ContentType =
            new MediaTypeHeaderValue(file.ContentType);

        content.Add(
            fileContent,
            "file",
            Path.GetFileName(file.FileName));

        return await client.PutAsync(endpoint, content);
    }
}