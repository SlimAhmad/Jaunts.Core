using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<ProviderCategory> ProviderCategory { get; set; }

        public async ValueTask<ProviderCategory> InsertProviderCategoryAsync(ProviderCategory category)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<ProviderCategory> categoryEntityEntry = await broker.ProviderCategory.AddAsync(entity: category);
            await broker.SaveChangesAsync();

            return categoryEntityEntry.Entity;
        }

        public IQueryable<ProviderCategory> SelectAllProviderCategories() => this.ProviderCategory;

        public async ValueTask<ProviderCategory> SelectProviderCategoryByIdAsync(Guid categoryId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await ProviderCategory.FindAsync(categoryId);
        }

        public async ValueTask<ProviderCategory> UpdateProviderCategoryAsync(ProviderCategory category)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<ProviderCategory> categoryEntityEntry = broker.ProviderCategory.Update(entity: category);
            await broker.SaveChangesAsync();

            return categoryEntityEntry.Entity;
        }

        public async ValueTask<ProviderCategory> DeleteProviderCategoryAsync(ProviderCategory category)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<ProviderCategory> categoryEntityEntry = broker.ProviderCategory.Remove(entity: category);
            await broker.SaveChangesAsync();

            return categoryEntityEntry.Entity;
        }
    }
}
