using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using NearbyOrgFinder.Data;
using NetTopologySuite.Geometries;

namespace NearbyOrgFinder.Services;

public interface IOrganizationsService
{
    public Task<OrganizationEntity> CreateOrganizationAsync(CreateOrganizationModel model, CancellationToken token = default);
    public Task DeleteOrganizationAsync(long organizationId, CancellationToken token = default);

    public Task<IEnumerable<OrganizationEntity>> GetOrganizationListByCityId(Guid cityId, OrganizationTypes type);
    
    public Task<OrganizationEntity> GetNearestOrganization(Point point, OrganizationTypes type);
    public Task<OrganizationEntity> UpdateOrganizationAsync(EditOrganizationModel model, long id, CancellationToken token = default);
}

public class OrganizationsService : IOrganizationsService
{
    private readonly GeoDbContext _geoDb;

    public OrganizationsService(GeoDbContext geoDbContext)
    {
        _geoDb = geoDbContext;
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
        
        var entry = _geoDb.Add(newOrganizationEntity);
        await _geoDb.SaveChangesAsync(token);
        return entry.Entity;
    }

    public async Task DeleteOrganizationAsync(long organizationId, CancellationToken token = default)
    {
        var entity = await _geoDb.Organizations.Where(_ => _.Id == organizationId).FirstOrDefaultAsync(token);
        if (entity is null) return;
        _geoDb.Remove(entity);
        await _geoDb.SaveChangesAsync(token);
    }

    public async Task<IEnumerable<OrganizationEntity>> GetOrganizationListByCityId(Guid cityId, OrganizationTypes type)
    {
        return  _geoDb.Organizations.Include(x => x.City)
            .Where(s => s.City.ExternalId == cityId && s.Type == type);
    }

    public async Task<OrganizationEntity> GetNearestOrganization(Point point, OrganizationTypes type) =>
        (await _geoDb.Organizations.Where(o => o.Type == type)
            .OrderBy(s => s.Location.Distance(point)).FirstOrDefaultAsync())!;

    public async Task<OrganizationEntity> UpdateOrganizationAsync(EditOrganizationModel model, long id, CancellationToken token = default)
    {
        var entity = await _geoDb.Organizations.Where(_ => _.Id == id).FirstOrDefaultAsync();

        if (entity is null) throw new Exception("Org was not found");
        
        entity.Name = model.Name;
        entity.Description = model.Description;
        entity.Location = new Point(model.Longtitude, model.Latitude);
        entity.Address = model.Address;
        entity.AddressComment = model.AddressComment;
        entity.MailIndex = model.MailIndex;
        entity.District = model.District;
        entity.WorkingHours = model.WorkingHours;
        entity.Timezone = model.Timezone;
        
        await _geoDb.SaveChangesAsync(token);
        return entity;
    }
}