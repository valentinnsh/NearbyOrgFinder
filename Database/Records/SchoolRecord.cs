using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace Database.Records;

public class SchoolRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("description")]
    public string Description { get; set; }
    
    [Column("address")]
    public string Address { get; set; }
    
    [Column("location")]
    public Point Location { get; set; }
    
    [Column("address_comment")]
    public string AddressComment { get; set; }
    
    [Column("mail_index")]
    public string MailIndex { get; set; }
    
    [Column("district")]
    public string District { get; set; }
    
    [Column("city_id")]
    public long CityId { get; set; }
    
    [Column("okrug")]
    public string Okrug { get; set; }
    
    [Column("region")]
    public string Region { get; set; }
    
    [Column("country")]
    public string Country { get; set; }
    
    [Column("working_hours")]
    public string WorkingHours { get; set; }
    
    [Column("timezone")]
    public string Timezone { get; set; }
    
    [Column("rating")]
    public double? Rating { get; set; }
    
    [Column("two_gis_url")]
    public string TwoGisUrl { get; set; }   
}
