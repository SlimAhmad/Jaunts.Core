using Jaunts.Core.Api.Models.Services.Foundations.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Driver> Driver { get; set; }

        public async ValueTask<Driver> InsertDriverAsync(Driver driver)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Driver> driverEntityEntry = await broker.Driver.AddAsync(entity: driver);
            await broker.SaveChangesAsync();

            return driverEntityEntry.Entity;
        }

        public IQueryable<Driver> SelectAllDrivers() => this.Driver;

        public async ValueTask<Driver> SelectDriverByIdAsync(Guid driverId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Driver.FindAsync(driverId);
        }

        public async ValueTask<Driver> UpdateDriverAsync(Driver driver)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Driver> driverEntityEntry = broker.Driver.Update(entity: driver);
            await broker.SaveChangesAsync();

            return driverEntityEntry.Entity;
        }

        public async ValueTask<Driver> DeleteDriverAsync(Driver driver)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Driver> driverEntityEntry = broker.Driver.Remove(entity: driver);
            await broker.SaveChangesAsync();

            return driverEntityEntry.Entity;
        }
    }
}
