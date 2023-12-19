using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using MyGeoApplication.Data;
using NetTopologySuite.Geometries;

namespace MyGeoApplication.Services;

public interface ISchoolsService
{
    public Task<SchoolEntity> CreateSchoolAsync(CreateSchoolModel model, CancellationToken token);
    public Task<SchoolEntity> DeleteSchoolAsync(DeleteSchoolModel model, CancellationToken token);

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
    
    public Task<SchoolEntity> CreateSchoolAsync(CreateSchoolModel model, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<SchoolEntity> DeleteSchoolAsync(DeleteSchoolModel model, CancellationToken token)
    {
        throw new NotImplementedException();
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