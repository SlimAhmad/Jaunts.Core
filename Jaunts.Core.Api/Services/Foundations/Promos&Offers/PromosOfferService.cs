// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.Promos_Offers;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOffers;

namespace Jaunts.Core.Api.Services.Foundations.PromosOffers
{
    public partial class PromosOfferService : IPromosOfferService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public PromosOfferService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<PromosOffer> RegisterPromosOfferAsync(PromosOffer driver) =>
        TryCatch(async () =>
        {
            ValidatePromosOfferOnRegister(driver);

            return await this.storageBroker.InsertPromosOfferAsync(driver);
        });

        public IQueryable<PromosOffer> RetrieveAllPromosOffers() =>
        TryCatch(() => this.storageBroker.SelectAllPromosOffers());

        public ValueTask<PromosOffer> RetrievePromosOfferByIdAsync(Guid driverId) =>
        TryCatch(async () =>
        {
            ValidatePromosOfferId(driverId);
            PromosOffer maybePromosOffer = await this.storageBroker.SelectPromosOffersByIdAsync(driverId);
            ValidateStoragePromosOffer(maybePromosOffer, driverId);

            return maybePromosOffer;
        });

        public ValueTask<PromosOffer> ModifyPromosOfferAsync(PromosOffer driver) =>
        TryCatch(async () =>
        {
            ValidatePromosOfferOnModify(driver);

            PromosOffer maybePromosOffer =
                await this.storageBroker.SelectPromosOffersByIdAsync(driver.Id);

            ValidateStoragePromosOffer(maybePromosOffer, driver.Id);
            ValidateAgainstStoragePromosOfferOnModify(inputPromosOffer: driver, storagePromosOffer: maybePromosOffer);

            return await this.storageBroker.UpdatePromosOffersAsync(driver);
        });

        public ValueTask<PromosOffer> RemovePromosOfferByIdAsync(Guid driverId) =>
        TryCatch(async () =>
        {
            ValidatePromosOfferId(driverId);

            PromosOffer maybePromosOffer =
                await this.storageBroker.SelectPromosOffersByIdAsync(driverId);

            ValidateStoragePromosOffer(maybePromosOffer, driverId);

            return await this.storageBroker.DeletePromosOffersAsync(maybePromosOffer);
        });

    }
}
