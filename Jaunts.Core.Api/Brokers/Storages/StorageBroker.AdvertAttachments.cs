using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<AdvertAttachment> AdvertAttachments { get; set; }

        public async ValueTask<AdvertAttachment> InsertAdvertAttachmentAsync(AdvertAttachment customer)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<AdvertAttachment> customerEntityEntry = await broker.AdvertAttachments.AddAsync(entity: customer);
            await broker.SaveChangesAsync();

            return customerEntityEntry.Entity;
        }

        public IQueryable<AdvertAttachment> SelectAllAdvertAttachments() => this.AdvertAttachments;

        public async ValueTask<AdvertAttachment> SelectAdvertAttachmentByIdAsync(
             Guid customerId,
             Guid attachmentId)
        {
            var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.AdvertAttachments.FindAsync(customerId, attachmentId);
        }

        public async ValueTask<AdvertAttachment> UpdateAdvertAttachmentAsync(AdvertAttachment customer)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<AdvertAttachment> customerEntityEntry = broker.AdvertAttachments.Update(entity: customer);
            await broker.SaveChangesAsync();

            return customerEntityEntry.Entity;
        }

        public async ValueTask<AdvertAttachment> DeleteAdvertAttachmentAsync(AdvertAttachment customer)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<AdvertAttachment> customerEntityEntry = broker.AdvertAttachments.Remove(entity: customer);
            await broker.SaveChangesAsync();

            return customerEntityEntry.Entity;
        }
    }
}
