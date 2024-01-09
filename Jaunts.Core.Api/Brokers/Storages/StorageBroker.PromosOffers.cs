using Jaunts.Core.Api.Models.Services.Foundations.Promos_Offers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<PromosOffer> PromosOffer { get; set; }

        public async ValueTask<PromosOffer> InsertPromosOfferAsync(PromosOffer promosOffer)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<PromosOffer> promosOfferEntityEntry = await broker.PromosOffer.AddAsync(entity: promosOffer);
            await broker.SaveChangesAsync();

            return promosOfferEntityEntry.Entity;
        }

        public IQueryable<PromosOffer> SelectAllPromosOffers() => this.PromosOffer;

        public async ValueTask<PromosOffer> SelectPromosOffersByIdAsync(Guid promosOfferId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await PromosOffer.FindAsync(promosOfferId);
        }

        public async ValueTask<PromosOffer> UpdatePromosOffersAsync(PromosOffer promosOffer)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<PromosOffer> promosOfferEntityEntry = broker.PromosOffer.Update(entity: promosOffer);
            await broker.SaveChangesAsync();

            return promosOfferEntityEntry.Entity;
        }

        public async ValueTask<PromosOffer> DeletePromosOffersAsync(PromosOffer promosOffer)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<PromosOffer> promosOfferEntityEntry = broker.PromosOffer.Remove(entity: promosOffer);
            await broker.SaveChangesAsync();

            return promosOfferEntityEntry.Entity;
        }
    }
}
