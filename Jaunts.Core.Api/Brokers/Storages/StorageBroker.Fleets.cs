using Jaunts.Core.Api.Models.Services.Foundations.Fleets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Fleet> Fleet { get; set; }

        public async ValueTask<Fleet> InsertFleetAsync(Fleet fleet)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Fleet> fleetEntityEntry = await broker.Fleet.AddAsync(entity: fleet);
            await broker.SaveChangesAsync();

            return fleetEntityEntry.Entity;
        }

        public IQueryable<Fleet> SelectAllFleets() => this.Fleet;

        public async ValueTask<Fleet> SelectFleetByIdAsync(Guid fleetId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Fleet.FindAsync(fleetId);
        }

        public async ValueTask<Fleet> UpdateFleetAsync(Fleet fleet)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Fleet> fleetEntityEntry = broker.Fleet.Update(entity: fleet);
            await broker.SaveChangesAsync();

            return fleetEntityEntry.Entity;
        }

        public async ValueTask<Fleet> DeleteFleetAsync(Fleet fleet)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Fleet> fleetEntityEntry = broker.Fleet.Remove(entity: fleet);
            await broker.SaveChangesAsync();

            return fleetEntityEntry.Entity;
        }
    }
}
