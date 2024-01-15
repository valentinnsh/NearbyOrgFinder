using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Database.Test;

public class ExistenceTest
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
    public async Task TryGettingSpbEntityFromDb()
    {
        var context = new DbContext();
        var test = await context.Cities.FirstAsync();
        test.Name.Should().Be("Saint-Petersburg");
    }
}