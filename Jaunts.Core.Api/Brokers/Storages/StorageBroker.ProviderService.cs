using Jaunts.Core.Api.Models.Services.Foundations.ProviderServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<ProviderService> ProviderService { get; set; }

        public async ValueTask<ProviderService> InsertProviderServiceAsync(ProviderService service)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<ProviderService> serviceEntityEntry = await broker.ProviderService.AddAsync(entity: service);
            await broker.SaveChangesAsync();

            return serviceEntityEntry.Entity;
        }

        public IQueryable<ProviderService> SelectAllProviderServices() => this.ProviderService;

        public async ValueTask<ProviderService> SelectProviderServiceByIdAsync(Guid serviceId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await ProviderService.FindAsync(serviceId);
        }

        public async ValueTask<ProviderService> UpdateProviderServiceAsync(ProviderService service)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<ProviderService> serviceEntityEntry = broker.ProviderService.Update(entity: service);
            await broker.SaveChangesAsync();

            return serviceEntityEntry.Entity;
        }

        public async ValueTask<ProviderService> DeleteProviderServiceAsync(ProviderService service)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<ProviderService> serviceEntityEntry = broker.ProviderService.Remove(entity: service);
            await broker.SaveChangesAsync();

            return serviceEntityEntry.Entity;
        }
    }
}
