// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.Providers;

namespace Jaunts.Core.Api.Services.Foundations.Providers
{
    public partial class ProviderService : IProviderService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ProviderService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Provider> CreateProviderAsync(Provider driver) =>
        TryCatch(async () =>
        {
            ValidateProviderOnRegister(driver);

            return await this.storageBroker.InsertProviderAsync(driver);
        });

        public IQueryable<Provider> RetrieveAllProviders() =>
        TryCatch(() => this.storageBroker.SelectAllProviders());

        public ValueTask<Provider> RetrieveProviderByIdAsync(Guid driverId) =>
        TryCatch(async () =>
        {
            ValidateProviderId(driverId);
            Provider maybeProvider = await this.storageBroker.SelectProviderByIdAsync(driverId);
            ValidateStorageProvider(maybeProvider, driverId);

            return maybeProvider;
        });

        public ValueTask<Provider> ModifyProviderAsync(Provider driver) =>
        TryCatch(async () =>
        {
            ValidateProviderOnModify(driver);

            Provider maybeProvider =
                await this.storageBroker.SelectProviderByIdAsync(driver.Id);

            ValidateStorageProvider(maybeProvider, driver.Id);
            ValidateAgainstStorageProviderOnModify(inputProvider: driver, storageProvider: maybeProvider);

            return await this.storageBroker.UpdateProviderAsync(driver);
        });

        public ValueTask<Provider> RemoveProviderByIdAsync(Guid driverId) =>
        TryCatch(async () =>
        {
            ValidateProviderId(driverId);

            Provider maybeProvider =
                await this.storageBroker.SelectProviderByIdAsync(driverId);

            ValidateStorageProvider(maybeProvider, driverId);

            return await this.storageBroker.DeleteProviderAsync(maybeProvider);
        });

    }
}
