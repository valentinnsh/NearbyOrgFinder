using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using MyGeoApplication.Data;
using NetTopologySuite.Geometries;

namespace MyGeoApplication.Services;

public interface ISchoolsService
{
    public Task<OrganizationEntity> CreateSchoolAsync(CreateSchoolModel model, CancellationToken token = default);
    public Task DeleteSchoolAsync(long schoolId, CancellationToken token = default);

    public Task<IEnumerable<OrganizationEntity>> GetSchoolListByCityId(Guid cityId);
    
    public Task<OrganizationEntity> GetNearestSchool(Point point);
}

public class SchoolsService : ISchoolsService
{
    private readonly MyGeoDbContext _db;

    public SchoolsService(MyGeoDbContext dbContext)
    {
        _db = dbContext;
    }
    
    public async Task<OrganizationEntity> CreateSchoolAsync(CreateSchoolModel model, CancellationToken token = default)
    {
        var newSchoolEntity = new OrganizationEntity
        {
            Name = model.Name,
            Description = model.Description,
            CityId = model.CityId,
            Location = new Point(model.Longtitude, model.Latitude),
            Address = model.Address,
            AddressComment = model.AddressComment,
            Type = OrganizationTypes.School
        };
        
        var entry = _db.Add(newSchoolEntity);
        await _db.SaveChangesAsync(token);
        return entry.Entity;
    }

    public async Task DeleteSchoolAsync(long schoolId, CancellationToken token = default)
    {
        var entity = await _db.Organizations.Where(_ => _.Id == schoolId).FirstOrDefaultAsync(token);
        if (entity is null) return;
        _db.Remove(entity);
        await _db.SaveChangesAsync(token);
    }

    public async Task<IEnumerable<OrganizationEntity>> GetSchoolListByCityId(Guid cityId)
    {
        return  _db.Organizations.Include(x => x.City)
            .Where(s => s.City.ExternalId == cityId && s.Type == OrganizationTypes.School);
    }

    public async Task<OrganizationEntity> GetNearestSchool(Point point)
    {
        var ordered =  await _db.Organizations.Where(o => o.Type == OrganizationTypes.School)
            .OrderBy(s => s.Location.Distance(point)).FirstOrDefaultAsync();
        return ordered;
    }
}