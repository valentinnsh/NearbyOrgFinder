using Database.Records;

namespace Database.Entities;

public class CityEntity: CityRecord
{
    public ICollection<OrganizationEntity> Organizations { get; set; }
}