using Jaunts.Core.Api.Models.Services.Foundations.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Provider> Provider { get; set; }

        public async ValueTask<Provider> InsertProviderAsync(Provider provider)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Provider> providerEntityEntry = await broker.Provider.AddAsync(entity: provider);
            await broker.SaveChangesAsync();

            return providerEntityEntry.Entity;
        }

        public IQueryable<Provider> SelectAllProviders() => this.Provider;

        public async ValueTask<Provider> SelectProviderByIdAsync(Guid providerId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Provider.FindAsync(providerId);
        }

        public async ValueTask<Provider> UpdateProviderAsync(Provider provider)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Provider> providerEntityEntry = broker.Provider.Update(entity: provider);
            await broker.SaveChangesAsync();

            return providerEntityEntry.Entity;
        }

        public async ValueTask<Provider> DeleteProviderAsync(Provider provider)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Provider> providerEntityEntry = broker.Provider.Remove(entity: provider);
            await broker.SaveChangesAsync();

            return providerEntityEntry.Entity;
        }
    }
}
