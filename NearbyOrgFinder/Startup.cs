using Database;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using NearbyOrgFinder.Services;
using NetTopologySuite.Geometries;
using Npgsql;
using Radzen;

namespace NearbyOrgFinder;

public class Startup
{
    public Startup()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, true)
            .AddJsonFile($"appsettings.Development.json", optional: true, true)
            .Build();
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<GeoDbContext>(opt => 
            opt.EnableSensitiveDataLogging().UseNpgsql(Configuration.GetConnectionString("PostgresConnection"),
                o => o.UseNetTopologySuite()));
        NpgsqlConnection.GlobalTypeMapper.UseGeoJson();

        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddScoped<DialogService>();
        services.AddScoped<NotificationService>();
        services.AddScoped<TooltipService>();
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<IOrganizationsService, OrganizationsService>();

        services.AddControllers(options =>
        {
            options.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider(typeof(Point)));
            options.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider(typeof(Coordinate)));
            options.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider(typeof(LineString)));
            options.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider(typeof(MultiLineString)));
        }).AddNewtonsoftJson(options =>
        {
            foreach (var converter in NetTopologySuite.IO.GeoJsonSerializer
                         .Create(new GeometryFactory(new PrecisionModel(), 4326)).Converters)
            {
                options.SerializerSettings.Converters.Add(converter);
            }
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
        });
    }
}