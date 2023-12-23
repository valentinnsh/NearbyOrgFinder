using System.ComponentModel.DataAnnotations.Schema;
using Database.Records;

namespace Database.Entities;

[Table("organizations")]
public class OrganizationEntity: OrganizationRecord
{
    public CityEntity City { get; set; }
}