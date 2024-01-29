using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Jaunts.Core.Api.Models.Services.Foundations.RideAttachments;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<RideAttachment> RideAttachments { get; set; }

        public async ValueTask<RideAttachment> InsertRideAttachmentAsync(RideAttachment rideAttachment)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<RideAttachment> rideAttachmentEntityEntry = await broker.RideAttachments.AddAsync(entity: rideAttachment);
            await broker.SaveChangesAsync();

            return rideAttachmentEntityEntry.Entity;
        }

        public IQueryable<RideAttachment> SelectAllRideAttachments() => this.RideAttachments;

        public async ValueTask<RideAttachment> SelectRideAttachmentByIdAsync(
             Guid rideAttachmentId,
             Guid attachmentId)
        {
            var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.RideAttachments.FindAsync(rideAttachmentId, attachmentId);
        }

        public async ValueTask<RideAttachment> UpdateRideAttachmentAsync(RideAttachment rideAttachment)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<RideAttachment> rideAttachmentEntityEntry = broker.RideAttachments.Update(entity: rideAttachment);
            await broker.SaveChangesAsync();

            return rideAttachmentEntityEntry.Entity;
        }

        public async ValueTask<RideAttachment> DeleteRideAttachmentAsync(RideAttachment rideAttachment)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<RideAttachment> rideAttachmentEntityEntry = broker.RideAttachments.Remove(entity: rideAttachment);
            await broker.SaveChangesAsync();

            return rideAttachmentEntityEntry.Entity;
        }
    }
}
