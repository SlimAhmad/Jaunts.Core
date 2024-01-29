using Jaunts.Core.Api.Models.Services.Foundations.Packages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Package> Package { get; set; }

        public async ValueTask<Package> InsertPackageAsync(Package package)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Package> packageEntityEntry = await broker.Package.AddAsync(entity: package);
            await broker.SaveChangesAsync();

            return packageEntityEntry.Entity;
        }

        public IQueryable<Package> SelectAllPackage() => this.Package;

        public async ValueTask<Package> SelectPackageByIdAsync(Guid packageId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Package.FindAsync(packageId);
        }

        public async ValueTask<Package> UpdatePackageAsync(Package package)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Package> packageEntityEntry = broker.Package.Update(entity: package);
            await broker.SaveChangesAsync();

            return packageEntityEntry.Entity;
        }

        public async ValueTask<Package> DeletePackageAsync(Package package)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Package> packageEntityEntry = broker.Package.Remove(entity: package);
            await broker.SaveChangesAsync();

            return packageEntityEntry.Entity;
        }
    }
}
