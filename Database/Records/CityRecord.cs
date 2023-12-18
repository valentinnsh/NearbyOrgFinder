using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace Database.Records;

public class CityRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; set; }
    [Column("external_id")]
    public Guid ExternalId { get; set; }
    [Column("name")]
    public string Name { get; set; }
    [Column("center")]
    public Point Center { get; set; }
}