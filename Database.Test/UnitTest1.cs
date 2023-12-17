using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Database.Test;

public class Tests
{
    private ConfigurationBuilder _config;

    [SetUp]
    public void Setup()
    {
        _config = new ConfigurationBuilder();
        var data = new Dictionary<string, string> { { "ConnectionStrings:GeoDb", "Host=localhost; Port=1; Database=geodb; Username=postgres; Password=geodb" } };
        _config.AddInMemoryCollection(data);
    }

    [Test]
    public async Task TryGettingInfoFromDefaultTable()
    {
        var context = new MyGeoDbContext(_config.Build());
        var test = await context.SpatialRefSysEntities.FirstAsync();
        test.Should().NotBeNull();
    }
}