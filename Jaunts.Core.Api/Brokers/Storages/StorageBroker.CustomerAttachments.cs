using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<CustomerAttachment> CustomerAttachments { get; set; }

        public async ValueTask<CustomerAttachment> InsertCustomerAttachmentAsync(CustomerAttachment customer)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<CustomerAttachment> customerEntityEntry = await broker.CustomerAttachments.AddAsync(entity: customer);
            await broker.SaveChangesAsync();

            return customerEntityEntry.Entity;
        }

        public IQueryable<CustomerAttachment> SelectAllCustomerAttachments() => this.CustomerAttachments;

        public async ValueTask<CustomerAttachment> SelectCustomerAttachmentByIdAsync(
             Guid customerId,
             Guid attachmentId)
        {
            var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.CustomerAttachments.FindAsync(customerId, attachmentId);
        }

        public async ValueTask<CustomerAttachment> UpdateCustomerAttachmentAsync(CustomerAttachment customer)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<CustomerAttachment> customerEntityEntry = broker.CustomerAttachments.Update(entity: customer);
            await broker.SaveChangesAsync();

            return customerEntityEntry.Entity;
        }

        public async ValueTask<CustomerAttachment> DeleteCustomerAttachmentAsync(CustomerAttachment customer)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<CustomerAttachment> customerEntityEntry = broker.CustomerAttachments.Remove(entity: customer);
            await broker.SaveChangesAsync();

            return customerEntityEntry.Entity;
        }
    }
}
