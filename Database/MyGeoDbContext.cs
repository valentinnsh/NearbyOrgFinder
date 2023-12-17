using Database.Entities;
using Database.Records;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace Database;

public class MyGeoDbContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public MyGeoDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(Configuration.GetConnectionString("GeoDb"));
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        //Configure default schema
        builder.HasDefaultSchema("public");
        OnCommonModelCreating(builder);
    }

    protected virtual void OnCommonModelCreating(ModelBuilder modelBuilder)
    {
       modelBuilder.Entity<SpatialRefSysEntity>().ToTable("spatial_ref_sys", "public");
    }
    public IQueryable<SpatialRefSysEntity> SpatialRefSysEntities => Set<SpatialRefSysEntity>();
}