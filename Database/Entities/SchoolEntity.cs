using System.ComponentModel.DataAnnotations.Schema;
using Database.Records;

namespace Database.Entities;

[Table("schools")]
public class SchoolEntity: SchoolRecord
{
    public CityEntity City { get; set; }
}