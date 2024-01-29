using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<PromosOfferAttachment> PromosOfferAttachments { get; set; }

        public async ValueTask<PromosOfferAttachment> InsertPromosOfferAttachmentAsync(PromosOfferAttachment customer)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<PromosOfferAttachment> customerEntityEntry = await broker.PromosOfferAttachments.AddAsync(entity: customer);
            await broker.SaveChangesAsync();

            return customerEntityEntry.Entity;
        }

        public IQueryable<PromosOfferAttachment> SelectAllPromosOfferAttachments() => this.PromosOfferAttachments;

        public async ValueTask<PromosOfferAttachment> SelectPromosOfferAttachmentByIdAsync(
             Guid customerId,
             Guid attachmentId)
        {
            var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.PromosOfferAttachments.FindAsync(customerId, attachmentId);
        }

        public async ValueTask<PromosOfferAttachment> UpdatePromosOfferAttachmentAsync(PromosOfferAttachment customer)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<PromosOfferAttachment> customerEntityEntry = broker.PromosOfferAttachments.Update(entity: customer);
            await broker.SaveChangesAsync();

            return customerEntityEntry.Entity;
        }

        public async ValueTask<PromosOfferAttachment> DeletePromosOfferAttachmentAsync(PromosOfferAttachment customer)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<PromosOfferAttachment> customerEntityEntry = broker.PromosOfferAttachments.Remove(entity: customer);
            await broker.SaveChangesAsync();

            return customerEntityEntry.Entity;
        }
    }
}
