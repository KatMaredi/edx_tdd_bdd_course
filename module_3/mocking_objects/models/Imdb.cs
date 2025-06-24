using mocking_objects.test;
using Newtonsoft.Json;

namespace mocking_objects.models;

public class Imdb
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<Imdb> _logger;
    private readonly string _apiKey;

    public Imdb(HttpClient httpClient, ILogger<Imdb> logger, string apiKey)
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiKey = apiKey;
    }

    public async Task<Dictionary<string, object>> SearchTitlesAsync(string title)
    {
        _logger.LogInformation($"Searching IMDb for Title: {title}.");

        var url = $"https://imdb-api.com/API/SearchTitle/{_apiKey}/{title}";
        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }

        return new Dictionary<string, object>();
    }

    public async Task<Dictionary<string, object>> MovieReviewsAsync(string id)
    {
        _logger.LogInformation($"Searching Imdb for reviews: {id}");

        var url = $"https://imdb-api.com/API/Reviews/{_apiKey}/{id}";
        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }

        return new Dictionary<string, object>();
    }

    public async Task<Dictionary<string, object>> MovieRatingAsync(string id)
    {
        _logger.LogInformation($"Searching imdb for reviews: {id}");

        var url = $"https://imdb-api.com/API/Ratings/{_apiKey}/{id}";
        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }

        return new Dictionary<string, object>();
    }
}