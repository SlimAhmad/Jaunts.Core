// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.Adverts;

namespace Jaunts.Core.Api.Services.Foundations.Adverts
{
    public partial class AdvertService : IAdvertService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public AdvertService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Advert> CreateAdvertAsync(Advert Advert) =>
        TryCatch(async () =>
        {
            ValidateAdvertOnRegister(Advert);

            return await this.storageBroker.InsertAdvertAsync(Advert);
        });

        public IQueryable<Advert> RetrieveAllAdverts() =>
        TryCatch(() => this.storageBroker.SelectAllAdverts());

        public ValueTask<Advert> RetrieveAdvertByIdAsync(Guid AdvertId) =>
        TryCatch(async () =>
        {
            ValidateAdvertId(AdvertId);
            Advert maybeAdvert = await this.storageBroker.SelectAdvertByIdAsync(AdvertId);
            ValidateStorageAdvert(maybeAdvert, AdvertId);

            return maybeAdvert;
        });

        public ValueTask<Advert> ModifyAdvertAsync(Advert Advert) =>
        TryCatch(async () =>
        {
            ValidateAdvertOnModify(Advert);

            Advert maybeAdvert =
                await this.storageBroker.SelectAdvertByIdAsync(Advert.Id);

            ValidateStorageAdvert(maybeAdvert, Advert.Id);
            ValidateAgainstStorageAdvertOnModify(inputAdvert: Advert, storageAdvert: maybeAdvert);

            return await this.storageBroker.UpdateAdvertAsync(Advert);
        });

        public ValueTask<Advert> RemoveAdvertByIdAsync(Guid AdvertId) =>
        TryCatch(async () =>
        {
            ValidateAdvertId(AdvertId);

            Advert maybeAdvert =
                await this.storageBroker.SelectAdvertByIdAsync(AdvertId);

            ValidateStorageAdvert(maybeAdvert, AdvertId);

            return await this.storageBroker.DeleteAdvertAsync(maybeAdvert);
        });

    }
}
