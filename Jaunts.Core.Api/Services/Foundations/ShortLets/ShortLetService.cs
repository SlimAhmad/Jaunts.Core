// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLets;

namespace Jaunts.Core.Api.Services.Foundations.ShortLets
{
    public partial class ShortLetService : IShortLetService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ShortLetService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<ShortLet> RegisterShortLetAsync(ShortLet shortlet) =>
        TryCatch(async () =>
        {
            ValidateShortLetOnRegister(shortlet);

            return await this.storageBroker.InsertShortLetAsync(shortlet);
        });

        public IQueryable<ShortLet> RetrieveAllShortLets() =>
        TryCatch(() => this.storageBroker.SelectAllShortLets());

        public ValueTask<ShortLet> RetrieveShortLetByIdAsync(Guid shortletId) =>
        TryCatch(async () =>
        {
            ValidateShortLetId(shortletId);
            ShortLet maybeShortLet = await this.storageBroker.SelectShortLetByIdAsync(shortletId);
            ValidateStorageShortLet(maybeShortLet, shortletId);

            return maybeShortLet;
        });

        public ValueTask<ShortLet> ModifyShortLetAsync(ShortLet shortlet) =>
        TryCatch(async () =>
        {
            ValidateShortLetOnModify(shortlet);

            ShortLet maybeShortLet =
                await this.storageBroker.SelectShortLetByIdAsync(shortlet.Id);

            ValidateStorageShortLet(maybeShortLet, shortlet.Id);
            ValidateAgainstStorageShortLetOnModify(inputShortLet: shortlet, storageShortLet: maybeShortLet);

            return await this.storageBroker.UpdateShortLetAsync(shortlet);
        });

        public ValueTask<ShortLet> RemoveShortLetByIdAsync(Guid shortletId) =>
        TryCatch(async () =>
        {
            ValidateShortLetId(shortletId);

            ShortLet maybeShortLet =
                await this.storageBroker.SelectShortLetByIdAsync(shortletId);

            ValidateStorageShortLet(maybeShortLet, shortletId);

            return await this.storageBroker.DeleteShortLetAsync(maybeShortLet);
        });

    }
}
