using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using NearbyOrgFinder.Data;
using NetTopologySuite.Geometries;
using DbContext = Database.DbContext;

namespace NearbyOrgFinder.Services;

public interface IPharmaciesService
{
    public Task<OrganizationEntity> CreatePharmacyAsync(CreatePharmacyModel model, CancellationToken token = default);
    public Task DeletePharmacyAsync(long PharmacyId, CancellationToken token = default);

    public Task<IEnumerable<OrganizationEntity>> GetPharmacyListByCityId(Guid cityId);
    
    public Task<OrganizationEntity> GetNearestPharmacy(Point point);
}

public class PharmaciesService : IPharmaciesService
{
    private readonly DbContext _db;

    public PharmaciesService(DbContext dbContext)
    {
        _db = dbContext;
    }
    
    public async Task<OrganizationEntity> CreatePharmacyAsync(CreatePharmacyModel model, CancellationToken token = default)
    {/*
        var newPharmacyEntity = new OrganizationEntity
        {
            Name = model.Name,
            Description = model.Description,
            CityId = model.CityId,
            Location = new Point(model.Longtitude, model.Latitude),
            Address = model.Address,
            AddressComment = model.AddressComment,
            Type = OrganizationTypes.Pharmacy
        };
        
        var entry = _db.Add(newPharmacyEntity);
        await _db.SaveChangesAsync(token);
        return entry.Entity;*/
        return new OrganizationEntity();
    }

    public async Task DeletePharmacyAsync(long PharmacyId, CancellationToken token = default)
    {
        var entity = await _db.Organizations.Where(_ => _.Id == PharmacyId).FirstOrDefaultAsync(token);
        if (entity is null) return;
        _db.Remove(entity);
        await _db.SaveChangesAsync(token);
    }

    public async Task<IEnumerable<OrganizationEntity>> GetPharmacyListByCityId(Guid cityId)
    {
        return  _db.Organizations.Include(x => x.City)
            .Where(s => s.City.ExternalId == cityId && s.Type == OrganizationTypes.Pharmacy);
    }

    public async Task<OrganizationEntity> GetNearestPharmacy(Point point)
    {
        var ordered =  await _db.Organizations.Where(o => o.Type == OrganizationTypes.Pharmacy)
            .OrderBy(s => s.Location.Distance(point)).FirstOrDefaultAsync();
        return ordered;
    }
}

public class CreatePharmacyModel
{
}