using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MyGeoApplication.Data;

namespace MyGeoApplication.Controllers;

[Route("api")]
public class PointsController : Controller
{
    [HttpGet("points.json")]
    public async Task<IActionResult> GetPointsJsonAsync()
    {
        var pointData = new PointData
        {
            features = new List<Feature>()
            {
                new() { geometry = new Geometry(30.345487, 59.945676) },
                new() { geometry = new Geometry(30.347488, 59.945676) },
                new() { geometry = new Geometry(30.345489, 59.945476) }
            }
        };

        return Content(JsonSerializer.Serialize(pointData), "application/json");
    }
}