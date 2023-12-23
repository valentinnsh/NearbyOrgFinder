using System.Configuration;
using System.Text.Json.Serialization;
using Database;
using GeoJSON.Net.Converters;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MyGeoApplication.Services;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using Npgsql;
using Radzen;

// I know this is kinda cringe, will refactor this in the future with startup and configuration
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyGeoDbContext>();
NpgsqlConnection.GlobalTypeMapper.UseGeoJson();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient<ICityService, CityService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5160/");
});
builder.WebHost.UseStaticWebAssets();
builder.Services.AddHttpClient();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ISchoolsService, SchoolsService>();
builder.Services.AddScoped<IPharmaciesService, PharmaciesService>();
builder.Services.AddScoped<IOrganizationsService, OrganizationsService>();


builder.Services.AddControllers(options =>
{
// Prevent the following exception: 'This method does not     support GeometryCollection arguments' 
// See: https://github.com/npgsql/Npgsql.EntityFrameworkCore.PostgreSQL/issues/585 
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
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();