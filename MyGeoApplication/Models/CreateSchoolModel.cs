namespace MyGeoApplication.Data;

public class CreateSchoolModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Latitude { get; set; }
    public double Longtitude { get; set; }
    public string Address { get; set; }

    public string AddressComment { get; set; }
    public string MailIndex { get; set; }
    public string District { get; set; }
    public long CityId { get; set; }
    public string Okrug { get; set; }
    public string Region { get; set; }
    public string Country { get; set; }
    public string WorkingHours { get; set; }
    public string Timezone { get; set; }
    public double? Rating { get; set; }
}