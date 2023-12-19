using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using MyGeoApplication.Data;
using NetTopologySuite.Geometries;

namespace MyGeoApplication.Services;

public interface ISchoolsService
{
    public Task<SchoolEntity> CreateSchoolAsync(CreateSchoolModel model, CancellationToken token = default);
    public Task DeleteSchoolAsync(long schoolId, CancellationToken token = default);

    public Task<IEnumerable<SchoolEntity>> GetSchoolListByCityId(Guid cityId);
    
    public Task<SchoolEntity> GetNearestSchool(Point point);
}

public class SchoolsService : ISchoolsService
{
    private readonly MyGeoDbContext _db;

    public SchoolsService(MyGeoDbContext dbContext)
    {
        _db = dbContext;
    }
    
    public async Task<SchoolEntity> CreateSchoolAsync(CreateSchoolModel model, CancellationToken token = default)
    {
        var newSchoolEntity = new SchoolEntity
        {
            Name = model.Name,
            Description = model.Description,
            CityId = model.CityId,
            Location = new Point(model.Longtitude, model.Latitude),
            Address = model.Address,
            AddressComment = model.AddressComment,
            
        };
        
        var entry = _db.Add(newSchoolEntity);
        await _db.SaveChangesAsync(token);
        return entry.Entity;
    }

    public async Task DeleteSchoolAsync(long schoolId, CancellationToken token = default)
    {
        var entity = await _db.Schools.Where(_ => _.Id == schoolId).FirstOrDefaultAsync(token);
        if (entity is null) return;
        _db.Remove(entity);
        await _db.SaveChangesAsync(token);
    }

    public async Task<IEnumerable<SchoolEntity>> GetSchoolListByCityId(Guid cityId)
    {
        return  _db.Schools.Include(x => x.City).Where(s => s.City.ExternalId == cityId);
    }

    public async Task<SchoolEntity> GetNearestSchool(Point point)
    {
        var ordered =  await _db.Schools.OrderBy(s => s.Location.Distance(point)).FirstOrDefaultAsync();
        return ordered;
    }
}