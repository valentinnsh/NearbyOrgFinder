using System.Runtime.Serialization;
using NetTopologySuite.Geometries;

namespace MyGeoApplication.Data;

[DataContract]
public class CityInfo
{
    [DataMember]
    public Guid ExternalId { get; set; }
    [DataMember]
    public string? Name { get; set; }
    [DataMember]
    public Point Center { get; set; }
}