using Jaunts.Core.Api.Models.Services.Foundations.Rides;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Ride> Ride { get; set; }

        public async ValueTask<Ride> InsertRideAsync(Ride ride)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Ride> rideEntityEntry = await broker.Ride.AddAsync(entity: ride);
            await broker.SaveChangesAsync();

            return rideEntityEntry.Entity;
        }

        public IQueryable<Ride> SelectAllRides() => this.Ride;

        public async ValueTask<Ride> SelectRideByIdAsync(Guid rideId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Ride.FindAsync(rideId);
        }

        public async ValueTask<Ride> UpdateRideAsync(Ride ride)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Ride> rideEntityEntry = broker.Ride.Update(entity: ride);
            await broker.SaveChangesAsync();

            return rideEntityEntry.Entity;
        }

        public async ValueTask<Ride> DeleteRideAsync(Ride ride)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Ride> rideEntityEntry = broker.Ride.Remove(entity: ride);
            await broker.SaveChangesAsync();

            return rideEntityEntry.Entity;
        }
    }
}
