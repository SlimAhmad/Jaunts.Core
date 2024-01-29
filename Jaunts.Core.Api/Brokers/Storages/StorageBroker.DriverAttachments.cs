using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<DriverAttachment> DriverAttachments { get; set; }

        public async ValueTask<DriverAttachment> InsertDriverAttachmentAsync(DriverAttachment customer)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<DriverAttachment> customerEntityEntry = await broker.DriverAttachments.AddAsync(entity: customer);
            await broker.SaveChangesAsync();

            return customerEntityEntry.Entity;
        }

        public IQueryable<DriverAttachment> SelectAllDriverAttachments() => this.DriverAttachments;

        public async ValueTask<DriverAttachment> SelectDriverAttachmentByIdAsync(
             Guid customerId,
             Guid attachmentId)
        {
            var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.DriverAttachments.FindAsync(customerId, attachmentId);
        }

        public async ValueTask<DriverAttachment> UpdateDriverAttachmentAsync(DriverAttachment customer)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<DriverAttachment> customerEntityEntry = broker.DriverAttachments.Update(entity: customer);
            await broker.SaveChangesAsync();

            return customerEntityEntry.Entity;
        }

        public async ValueTask<DriverAttachment> DeleteDriverAttachmentAsync(DriverAttachment customer)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<DriverAttachment> customerEntityEntry = broker.DriverAttachments.Remove(entity: customer);
            await broker.SaveChangesAsync();

            return customerEntityEntry.Entity;
        }
    }
}
