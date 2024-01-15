using NearbyOrgFinder.Data;
using Newtonsoft.Json;

namespace NearbyOrgFinder.Services;

public interface ICityService
{
    public Task<CityInfo> GetCityByIdAsync(string id);
}

public class CityService : ICityService
{
    private readonly HttpClient _httpClient;

    public CityService(HttpClient client)
    {
        _httpClient = client;
    }

    public async Task<CityInfo> GetCityByIdAsync(string id)
    {
        var response = await _httpClient.GetAsync($"api/cities/{id}");
        response.EnsureSuccessStatusCode();
        var stream = await response.Content.ReadAsStreamAsync();
        return JsonExtensions.LoadFromStreamWithGeoJson<CityInfo>(stream);
    }
}