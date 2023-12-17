using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Records;

public class SpatialRefSysRecord
{
    [Key]
    [Column("srid")]
    public int SRID { get; set; }
    [Column("auth_name")]
    public string AuthName { get; set; }
    [Column("auth_srid")]
    public int AuthSRID { get; set; }
    [Column("srtext")]
    public string SRText { get; set; }
    [Column("proj4text")]
    public string Proj4Text { get; set; }
}
