using Database;
using Microsoft.EntityFrameworkCore;
using NearbyOrgFinder.Data;

namespace NearbyOrgFinder.Services;

public interface ICityService
{
    public Task<CityInfo> GetCityByIdAsync(string id);
}

public class CityService : ICityService
{
    private readonly GeoDbContext _geoDb;
    public CityService(GeoDbContext context)
    {
        _geoDb = context;
    }

    public async Task<CityInfo> GetCityByIdAsync(string id)
    {
        var city = await _geoDb.Cities
            .Where(c => c.ExternalId == Guid.Parse(id))
            .Select(a => new CityInfo
            {
                ExternalId = a.ExternalId,
                Center = a.Center,
                Name = a.Name
            }).FirstOrDefaultAsync();
        if (city is null) throw new Exception("City was not found");
        return city;
    }
}