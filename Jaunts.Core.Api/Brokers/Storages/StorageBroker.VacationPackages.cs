using Jaunts.Core.Api.Models.Services.Foundations.VacationPackages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<VacationPackage> VacationPackage { get; set; }

        public async ValueTask<VacationPackage> InsertVacationPackageAsync(VacationPackage package)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<VacationPackage> packageEntityEntry = await broker.VacationPackage.AddAsync(entity: package);
            await broker.SaveChangesAsync();

            return packageEntityEntry.Entity;
        }

        public IQueryable<VacationPackage> SelectAllVacationPackages() => this.VacationPackage;

        public async ValueTask<VacationPackage> SelectVacationPackageByIdAsync(Guid packageId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await VacationPackage.FindAsync(packageId);
        }

        public async ValueTask<VacationPackage> UpdateVacationPackageAsync(VacationPackage package)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<VacationPackage> packageEntityEntry = broker.VacationPackage.Update(entity: package);
            await broker.SaveChangesAsync();

            return packageEntityEntry.Entity;
        }

        public async ValueTask<VacationPackage> DeleteVacationPackageAsync(VacationPackage package)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<VacationPackage> packageEntityEntry = broker.VacationPackage.Remove(entity: package);
            await broker.SaveChangesAsync();

            return packageEntityEntry.Entity;
        }
    }
}
