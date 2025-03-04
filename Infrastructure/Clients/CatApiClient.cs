using System.Text.Json;
using Application.Interfaces;
using Models.InfrastructureModels;

namespace Infrastructure.Clients;

public class CatApiClient(string? apiUrl) : ICatApiClient
{
    private readonly HttpClient _httpClient = new();
    private string? _apiUrl = apiUrl;

    public async Task<List<CatApiResponse>> FetchRandomCats(int? pageSize = 5)
    {
        if (string.IsNullOrEmpty(_apiUrl))
            throw new InvalidOperationException("Cats API Url is not configured.");
        
        if (pageSize is not null)
            _apiUrl = $"{_apiUrl}&limit={pageSize}";

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