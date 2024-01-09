using Jaunts.Core.Api.Models.Services.Foundations.Promos_Offers;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<PromosOffer> InsertPromosOfferAsync(
            PromosOffer promosOffers);

        IQueryable<PromosOffer> SelectAllPromosOffers();

        ValueTask<PromosOffer> SelectPromosOffersByIdAsync(
            Guid promoOfferId);

        ValueTask<PromosOffer> UpdatePromosOffersAsync(
            PromosOffer promosOffers);

        ValueTask<PromosOffer> DeletePromosOffersAsync(
            PromosOffer promosOffers);
    }
}
