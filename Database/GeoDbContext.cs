using Database.Entities;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Database;

public class GeoDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public GeoDbContext(DbContextOptions<GeoDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasPostgresExtension("postgis");
        OnCommonModelCreating(builder);
        OnSeed(builder);
        SeedSchools.FullSeedSchools(builder);
        SeedPharmacy.FullSeedPharmacy(builder);
        SeedVet.FullSeedVet(builder);
    }

    protected void OnCommonModelCreating(ModelBuilder modelBuilder)
    {
       var city = modelBuilder.Entity<CityEntity>().ToTable("cities", "geo_data");
       city.Property(p => p.ExternalId).ValueGeneratedOnAdd();
       
       var organizations = modelBuilder.Entity<OrganizationEntity>().ToTable("organizations", "geo_data");

       modelBuilder.Entity<CityEntity>().HasMany(x => x.Organizations)
           .WithOne(x => x.City)
           .HasForeignKey(s => s.CityId)
           .HasPrincipalKey(s => s.Id);
       
    }

    protected void OnSeed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrganizationEntity>().HasData(
            new OrganizationEntity
            {
                Id = 1,
                Name = "Крылья",
                Description = "частная школа",
                Address = "Большая Пушкарская, 35",
                Location = new Point(30.308695, 59.961798),
                AddressComment = "",
                MailIndex = "197101",
                District = "Петроградский район",
                CityId = 1,
                Okrug = "",
                Region = "Санкт-Петербург",
                Country = "Россия",
                WorkingHours =
                    "Пн: 08:00-20:00; Вт: 08:00-20:00; Ср: 08:00-20:00; Чт: 08:00-20:00; Пт: 08:00-20:00",
                Timezone = "+03:00",
                Rating = 5,
                TwoGisUrl = "https://2gis.com/firm/70000001068931674"
            },
            new OrganizationEntity
            {
                Id = 2,
                Name = "Ювента",
                Description = "частная школа",
                Address = "Средний проспект В.О., 28",
                Location = new Point(30.279865, 59.942844),
                AddressComment = "2 этаж",
                MailIndex = "199004",
                District = "Василеостровский район",
                CityId = 1,
                Okrug = "",
                Region = "Санкт-Петербург",
                Country = "Россия",
                WorkingHours =
                    "Пн: 09:00-18:00; Вт: 09:00-18:00; Ср: 09:00-18:00; Чт: 09:00-18:00; Пт: 09:00-18:00",
                Timezone = "+03:00",
                Rating = 4,
                TwoGisUrl = "https://2gis.com/firm/5348552838706876"
            });
    }
    public IQueryable<CityEntity> Cities => Set<CityEntity>();
    public IQueryable<OrganizationEntity> Organizations => Set<OrganizationEntity>();
}