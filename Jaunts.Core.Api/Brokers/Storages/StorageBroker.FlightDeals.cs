using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<FlightDeal> FlightDeal { get; set; }

        public async ValueTask<FlightDeal> InsertFlightDealAsync(FlightDeal FlightDeal)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<FlightDeal> FlightDealEntityEntry = await broker.FlightDeal.AddAsync(entity: FlightDeal);
            await broker.SaveChangesAsync();

            return FlightDealEntityEntry.Entity;
        }

        public IQueryable<FlightDeal> SelectAllFlightDeals() => this.FlightDeal;

        public async ValueTask<FlightDeal> SelectFlightDealByIdAsync(Guid FlightDealId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await FlightDeal.FindAsync(FlightDealId);
        }

        public async ValueTask<FlightDeal> UpdateFlightDealAsync(FlightDeal FlightDeal)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<FlightDeal> FlightDealEntityEntry = broker.FlightDeal.Update(entity: FlightDeal);
            await broker.SaveChangesAsync();

            return FlightDealEntityEntry.Entity;
        }

        public async ValueTask<FlightDeal> DeleteFlightDealAsync(FlightDeal FlightDeal)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<FlightDeal> FlightDealEntityEntry = broker.FlightDeal.Remove(entity: FlightDeal);
            await broker.SaveChangesAsync();

            return FlightDealEntityEntry.Entity;
        }
    }
}
