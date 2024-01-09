using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<ProviderAttachment> ProviderAttachments { get; set; }

        public async ValueTask<ProviderAttachment> InsertProviderAttachmentAsync(ProviderAttachment providerAttachment)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<ProviderAttachment> providerAttachmentEntityEntry = await broker.ProviderAttachments.AddAsync(entity: providerAttachment);
            await broker.SaveChangesAsync();

            return providerAttachmentEntityEntry.Entity;
        }

        public IQueryable<ProviderAttachment> SelectAllProviderAttachments() => this.ProviderAttachments;

        public async ValueTask<ProviderAttachment> SelectProviderAttachmentByIdAsync(
             Guid providerAttachmentId,
             Guid attachmentId)
        {
            var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.ProviderAttachments.FindAsync(providerAttachmentId, attachmentId);
        }

        public async ValueTask<ProviderAttachment> UpdateProviderAttachmentAsync(ProviderAttachment providerAttachment)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<ProviderAttachment> providerAttachmentEntityEntry = broker.ProviderAttachments.Update(entity: providerAttachment);
            await broker.SaveChangesAsync();

            return providerAttachmentEntityEntry.Entity;
        }

        public async ValueTask<ProviderAttachment> DeleteProviderAttachmentAsync(ProviderAttachment providerAttachment)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<ProviderAttachment> providerAttachmentEntityEntry = broker.ProviderAttachments.Remove(entity: providerAttachment);
            await broker.SaveChangesAsync();

            return providerAttachmentEntityEntry.Entity;
        }
    }
}
