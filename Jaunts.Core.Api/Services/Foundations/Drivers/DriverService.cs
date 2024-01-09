// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.Drivers;

namespace Jaunts.Core.Api.Services.Foundations.Drivers
{
    public partial class DriverService : IDriverService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public DriverService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Driver> RegisterDriverAsync(Driver driver) =>
        TryCatch(async () =>
        {
            ValidateDriverOnRegister(driver);

            return await this.storageBroker.InsertDriverAsync(driver);
        });

        public IQueryable<Driver> RetrieveAllDrivers() =>
        TryCatch(() => this.storageBroker.SelectAllDrivers());

        public ValueTask<Driver> RetrieveDriverByIdAsync(Guid driverId) =>
        TryCatch(async () =>
        {
            ValidateDriverId(driverId);
            Driver maybeDriver = await this.storageBroker.SelectDriverByIdAsync(driverId);
            ValidateStorageDriver(maybeDriver, driverId);

            return maybeDriver;
        });

        public ValueTask<Driver> ModifyDriverAsync(Driver driver) =>
        TryCatch(async () =>
        {
            ValidateDriverOnModify(driver);

            Driver maybeDriver =
                await this.storageBroker.SelectDriverByIdAsync(driver.Id);

            ValidateStorageDriver(maybeDriver, driver.Id);
            ValidateAgainstStorageDriverOnModify(inputDriver: driver, storageDriver: maybeDriver);

            return await this.storageBroker.UpdateDriverAsync(driver);
        });

        public ValueTask<Driver> RemoveDriverByIdAsync(Guid driverId) =>
        TryCatch(async () =>
        {
            ValidateDriverId(driverId);

            Driver maybeDriver =
                await this.storageBroker.SelectDriverByIdAsync(driverId);

            ValidateStorageDriver(maybeDriver, driverId);

            return await this.storageBroker.DeleteDriverAsync(maybeDriver);
        });

    }
}
