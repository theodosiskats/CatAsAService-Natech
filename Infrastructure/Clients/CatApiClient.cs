using System.Text.Json;
using Application.Interfaces;
using Models.InfrastructureModels;

namespace Infrastructure.Clients;

public class CatApiClient : ICatApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string? _apiUrl;
    
    public CatApiClient(string? apiUrl, string? apiKey)
    {
        _apiUrl = apiUrl;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
    }

    public async Task<List<CatApiResponse>> FetchRandomCats()
    {
        if (string.IsNullOrEmpty(_apiUrl))
            throw new InvalidOperationException("Cats API Url is not configured.");

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_apiUrl);
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var catApiResponse = JsonSerializer.Deserialize<List<CatApiResponse>>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return catApiResponse ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching cats: {ex.Message}");
            return [];
        }
    }
}