using Jaunts.Core.Api.Models.Services.Foundations.Amenities;
using Jaunts.Core.Api.Models.Services.Foundations.Amenitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Amenity> Amenities { get; set; }

        public async ValueTask<Amenity> InsertAmenityAsync(Amenity amenity)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Amenity> amenityEntityEntry = await broker.Amenities.AddAsync(entity: amenity);
            await broker.SaveChangesAsync();

            return amenityEntityEntry.Entity;
        }

        public IQueryable<Amenity> SelectAllAmenities() => this.Amenities;

        public async ValueTask<Amenity> SelectAmenityByIdAsync(Guid amenityId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Amenities.FindAsync(amenityId);
        }

        public async ValueTask<Amenity> UpdateAmenityAsync(Amenity amenity)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Amenity> amenityEntityEntry = broker.Amenities.Update(entity: amenity);
            await broker.SaveChangesAsync();

            return amenityEntityEntry.Entity;
        }

        public async ValueTask<Amenity> DeleteAmenityAsync(Amenity amenity)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Amenity> amenityEntityEntry = broker.Amenities.Remove(entity: amenity);
            await broker.SaveChangesAsync();

            return amenityEntityEntry.Entity;
        }
    }
}
