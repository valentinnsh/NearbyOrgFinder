using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using NearbyOrgFinder.Data;
using NetTopologySuite.Geometries;
using DbContext = Database.DbContext;

namespace NearbyOrgFinder.Services;

public interface IOrganizationsService
{
    public Task<OrganizationEntity> CreateOrganizationAsync(CreateOrganizationModel model, CancellationToken token = default);
    public Task DeleteOrganizationAsync(long organizationId, CancellationToken token = default);

    public Task<IEnumerable<OrganizationEntity>> GetOrganizationListByCityId(Guid cityId, OrganizationTypes type);
    
    public Task<OrganizationEntity> GetNearestOrganization(Point point, OrganizationTypes type);
}

public class OrganizationsService : IOrganizationsService
{
    private readonly DbContext _db;

    public OrganizationsService(DbContext dbContext)
    {
        _db = dbContext;
    }
    
    public async Task<OrganizationEntity> CreateOrganizationAsync(CreateOrganizationModel model,CancellationToken token = default)
    {
        var newOrganizationEntity = new OrganizationEntity
        {
            Name = model.Name,
            Description = model.Description,
            CityId = model.CityId,
            Location = new Point(model.Longtitude, model.Latitude),
            Address = model.Address,
            AddressComment = model.AddressComment,
            Type = model.Type
        };
        
        var entry = _db.Add(newOrganizationEntity);
        await _db.SaveChangesAsync(token);
        return entry.Entity;
    }

    public async Task DeleteOrganizationAsync(long organizationId, CancellationToken token = default)
    {
        var entity = await _db.Organizations.Where(_ => _.Id == organizationId).FirstOrDefaultAsync(token);
        if (entity is null) return;
        _db.Remove(entity);
        await _db.SaveChangesAsync(token);
    }

    public async Task<IEnumerable<OrganizationEntity>> GetOrganizationListByCityId(Guid cityId, OrganizationTypes type)
    {
        return  _db.Organizations.Include(x => x.City)
            .Where(s => s.City.ExternalId == cityId && s.Type == type);
    }

    public async Task<OrganizationEntity> GetNearestOrganization(Point point, OrganizationTypes type) =>
        (await _db.Organizations.Where(o => o.Type == type)
            .OrderBy(s => s.Location.Distance(point)).FirstOrDefaultAsync())!;
}