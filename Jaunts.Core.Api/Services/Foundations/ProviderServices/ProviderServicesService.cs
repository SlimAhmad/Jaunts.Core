// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderServices;

namespace Jaunts.Core.Api.Services.Foundations.ProviderServices
{
    public partial class ProviderServicesService : IProviderServicesService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ProviderServicesService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<ProviderService> RegisterProviderServiceAsync(ProviderService driver) =>
        TryCatch(async () =>
        {
            ValidateProviderServiceOnRegister(driver);

            return await this.storageBroker.InsertProviderServiceAsync(driver);
        });

        public IQueryable<ProviderService> RetrieveAllProviderServices() =>
        TryCatch(() => this.storageBroker.SelectAllProviderServices());

        public ValueTask<ProviderService> RetrieveProviderServiceByIdAsync(Guid driverId) =>
        TryCatch(async () =>
        {
            ValidateProviderServiceId(driverId);
            ProviderService maybeProviderService = await this.storageBroker.SelectProviderServiceByIdAsync(driverId);
            ValidateStorageProviderService(maybeProviderService, driverId);

            return maybeProviderService;
        });

        public ValueTask<ProviderService> ModifyProviderServiceAsync(ProviderService driver) =>
        TryCatch(async () =>
        {
            ValidateProviderServiceOnModify(driver);

            ProviderService maybeProviderService =
                await this.storageBroker.SelectProviderServiceByIdAsync(driver.Id);

            ValidateStorageProviderService(maybeProviderService, driver.Id);
            ValidateAgainstStorageProviderServiceOnModify(inputProviderService: driver, storageProviderService: maybeProviderService);

            return await this.storageBroker.UpdateProviderServiceAsync(driver);
        });

        public ValueTask<ProviderService> RemoveProviderServiceByIdAsync(Guid driverId) =>
        TryCatch(async () =>
        {
            ValidateProviderServiceId(driverId);

            ProviderService maybeProviderService =
                await this.storageBroker.SelectProviderServiceByIdAsync(driverId);

            ValidateStorageProviderService(maybeProviderService, driverId);

            return await this.storageBroker.DeleteProviderServiceAsync(maybeProviderService);
        });

    }
}
