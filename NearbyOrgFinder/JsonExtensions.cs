using Newtonsoft.Json;

namespace NearbyOrgFinder;

public static partial class JsonExtensions
{
    public static T LoadFromStreamWithGeoJson<T>(Stream stream, JsonSerializerSettings settings = null)
    {
        var serializer = NetTopologySuite.IO.GeoJsonSerializer.CreateDefault(settings);
        serializer.CheckAdditionalContent = true;
        using (var textreader = new StreamReader(stream))
        using (var jsonreader = new JsonTextReader(textreader))
        {
            return serializer.Deserialize<T>(jsonreader);
        }
    }
}