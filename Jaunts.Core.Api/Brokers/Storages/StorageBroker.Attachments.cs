using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Attachment> Attachments { get; set; }

        public async ValueTask<Attachment> InsertAttachmentAsync(Attachment attachment)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Attachment> attachmentEntityEntry = await broker.Attachments.AddAsync(entity: attachment);
            await broker.SaveChangesAsync();

            return attachmentEntityEntry.Entity;
        }

        public IQueryable<Attachment> SelectAllAttachments() => this.Attachments;

        public async ValueTask<Attachment> SelectAttachmentByIdAsync(Guid attachmentId)
        {
            var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Attachments.FindAsync(attachmentId);
        }

        public async ValueTask<Attachment> UpdateAttachmentAsync(Attachment attachment)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Attachment> attachmentEntityEntry = broker.Attachments.Update(entity: attachment);
            await broker.SaveChangesAsync();

            return attachmentEntityEntry.Entity;
        }

        public async ValueTask<Attachment> DeleteAttachmentAsync(Attachment attachment)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Attachment> attachmentEntityEntry = broker.Attachments.Remove(entity: attachment);
            await broker.SaveChangesAsync();

            return attachmentEntityEntry.Entity;
        }
    }
}
