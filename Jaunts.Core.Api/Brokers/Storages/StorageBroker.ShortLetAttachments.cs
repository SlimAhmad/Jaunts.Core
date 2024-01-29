using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<ShortLetAttachment> ShortLetAttachments { get; set; }

        public async ValueTask<ShortLetAttachment> InsertShortLetAttachmentAsync(ShortLetAttachment rideAttachment)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<ShortLetAttachment> rideAttachmentEntityEntry = await broker.ShortLetAttachments.AddAsync(entity: rideAttachment);
            await broker.SaveChangesAsync();

            return rideAttachmentEntityEntry.Entity;
        }

        public IQueryable<ShortLetAttachment> SelectAllShortLetAttachments() => this.ShortLetAttachments;

        public async ValueTask<ShortLetAttachment> SelectShortLetAttachmentByIdAsync(
             Guid rideAttachmentId,
             Guid attachmentId)
        {
            var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.ShortLetAttachments.FindAsync(rideAttachmentId, attachmentId);
        }

        public async ValueTask<ShortLetAttachment> UpdateShortLetAttachmentAsync(ShortLetAttachment rideAttachment)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<ShortLetAttachment> rideAttachmentEntityEntry = broker.ShortLetAttachments.Update(entity: rideAttachment);
            await broker.SaveChangesAsync();

            return rideAttachmentEntityEntry.Entity;
        }

        public async ValueTask<ShortLetAttachment> DeleteShortLetAttachmentAsync(ShortLetAttachment rideAttachment)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<ShortLetAttachment> rideAttachmentEntityEntry = broker.ShortLetAttachments.Remove(entity: rideAttachment);
            await broker.SaveChangesAsync();

            return rideAttachmentEntityEntry.Entity;
        }
    }
}
