using Database.Records;

namespace Database.Entities;

public class CityEntity: CityRecord
{
    public ICollection<SchoolEntity> Schools { get; set; }
}