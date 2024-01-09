// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Promos_Offers;

namespace Jaunts.Core.Api.Services.Foundations.PromosOffers
{
    public interface IPromosOfferService
    {
        ValueTask<PromosOffer> RegisterPromosOfferAsync(PromosOffer PromosOffer);
        IQueryable<PromosOffer> RetrieveAllPromosOffers();
        ValueTask<PromosOffer> RetrievePromosOfferByIdAsync(Guid PromosOfferId); 
        ValueTask<PromosOffer> ModifyPromosOfferAsync(PromosOffer PromosOffer);
        ValueTask<PromosOffer> RemovePromosOfferByIdAsync(Guid PromosOfferId);
    }
}