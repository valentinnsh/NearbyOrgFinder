using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace Database;

public class MyGeoDbContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public MyGeoDbContext()
    {
        // Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql("Host=localhost; Port=1; Database=geodb; Username=postgres; Password=geodb", 
            o => o.UseNetTopologySuite());
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasPostgresExtension("postgis");
        OnCommonModelCreating(builder);
    }

    protected virtual void OnCommonModelCreating(ModelBuilder modelBuilder)
    {
       modelBuilder.Entity<CityEntity>().ToTable("cities", "geo_data");
       modelBuilder.Entity<CityEntity>().Property(p => p.ExternalId).ValueGeneratedOnAdd();
    }
    public IQueryable<CityEntity> Cities => Set<CityEntity>();

    internal DbSet<CityEntity> CitiesSet { get; set; }
}