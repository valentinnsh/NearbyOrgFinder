namespace MyGeoApplication.Data;

public class PointData
{
    public string type { get; set; } = "FeatureCollection";
    public List<Feature> features { get; set; }
}

public class Feature
{
    public string type { get; set; } = "Feature";
    public Geometry geometry { get; set; }
    public Properties properties { get; set; } = new() { name = "Unnamed" };
}

public class Geometry
{
    public string type { get; set; } = "Point";
    public List<double> coordinates { get; set; }

    public Geometry(double latitude, double longtitide)
    {
        coordinates = new List<double>(){ latitude, longtitide };
    }
}

public class Properties
{
    public string name { get; set; }
}