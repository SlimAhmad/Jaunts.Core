using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<FlightDealAttachment> FlightDealAttachments { get; set; }

        public async ValueTask<FlightDealAttachment> InsertFlightDealAttachmentAsync(FlightDealAttachment customer)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<FlightDealAttachment> customerEntityEntry = await broker.FlightDealAttachments.AddAsync(entity: customer);
            await broker.SaveChangesAsync();

            return customerEntityEntry.Entity;
        }

        public IQueryable<FlightDealAttachment> SelectAllFlightDealAttachments() => this.FlightDealAttachments;

        public async ValueTask<FlightDealAttachment> SelectFlightDealAttachmentByIdAsync(
             Guid customerId,
             Guid attachmentId)
        {
            var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.FlightDealAttachments.FindAsync(customerId, attachmentId);
        }

        public async ValueTask<FlightDealAttachment> UpdateFlightDealAttachmentAsync(FlightDealAttachment customer)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<FlightDealAttachment> customerEntityEntry = broker.FlightDealAttachments.Update(entity: customer);
            await broker.SaveChangesAsync();

            return customerEntityEntry.Entity;
        }

        public async ValueTask<FlightDealAttachment> DeleteFlightDealAttachmentAsync(FlightDealAttachment customer)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<FlightDealAttachment> customerEntityEntry = broker.FlightDealAttachments.Remove(entity: customer);
            await broker.SaveChangesAsync();

            return customerEntityEntry.Entity;
        }
    }
}
