// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.Fleets;

namespace Jaunts.Core.Api.Services.Foundations.Fleets
{
    public partial class FleetService : IFleetService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public FleetService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Fleet> CreateFleetAsync(Fleet driver) =>
        TryCatch(async () =>
        {
            ValidateFleetOnRegister(driver);

            return await this.storageBroker.InsertFleetAsync(driver);
        });

        public IQueryable<Fleet> RetrieveAllFleets() =>
        TryCatch(() => this.storageBroker.SelectAllFleets());

        public ValueTask<Fleet> RetrieveFleetByIdAsync(Guid driverId) =>
        TryCatch(async () =>
        {
            ValidateFleetId(driverId);
            Fleet maybeFleet = await this.storageBroker.SelectFleetByIdAsync(driverId);
            ValidateStorageFleet(maybeFleet, driverId);

            return maybeFleet;
        });

        public ValueTask<Fleet> ModifyFleetAsync(Fleet driver) =>
        TryCatch(async () =>
        {
            ValidateFleetOnModify(driver);

            Fleet maybeFleet =
                await this.storageBroker.SelectFleetByIdAsync(driver.Id);

            ValidateStorageFleet(maybeFleet, driver.Id);
            ValidateAgainstStorageFleetOnModify(inputFleet: driver, storageFleet: maybeFleet);

            return await this.storageBroker.UpdateFleetAsync(driver);
        });

        public ValueTask<Fleet> RemoveFleetByIdAsync(Guid driverId) =>
        TryCatch(async () =>
        {
            ValidateFleetId(driverId);

            Fleet maybeFleet =
                await this.storageBroker.SelectFleetByIdAsync(driverId);

            ValidateStorageFleet(maybeFleet, driverId);

            return await this.storageBroker.DeleteFleetAsync(maybeFleet);
        });

    }
}
