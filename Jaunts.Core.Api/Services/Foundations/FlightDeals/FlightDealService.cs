// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals;

namespace Jaunts.Core.Api.Services.Foundations.FlightDeals
{
    public partial class FlightDealService : IFlightDealService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public FlightDealService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<FlightDeal> CreateFlightDealAsync(FlightDeal driver) =>
        TryCatch(async () =>
        {
            ValidateFlightDealOnRegister(driver);

            return await this.storageBroker.InsertFlightDealAsync(driver);
        });

        public IQueryable<FlightDeal> RetrieveAllFlightDeals() =>
        TryCatch(() => this.storageBroker.SelectAllFlightDeals());

        public ValueTask<FlightDeal> RetrieveFlightDealByIdAsync(Guid driverId) =>
        TryCatch(async () =>
        {
            ValidateFlightDealId(driverId);
            FlightDeal maybeFlightDeal = await this.storageBroker.SelectFlightDealByIdAsync(driverId);
            ValidateStorageFlightDeal(maybeFlightDeal, driverId);

            return maybeFlightDeal;
        });

        public ValueTask<FlightDeal> ModifyFlightDealAsync(FlightDeal driver) =>
        TryCatch(async () =>
        {
            ValidateFlightDealOnModify(driver);

            FlightDeal maybeFlightDeal =
                await this.storageBroker.SelectFlightDealByIdAsync(driver.Id);

            ValidateStorageFlightDeal(maybeFlightDeal, driver.Id);
            ValidateAgainstStorageFlightDealOnModify(inputFlightDeal: driver, storageFlightDeal: maybeFlightDeal);

            return await this.storageBroker.UpdateFlightDealAsync(driver);
        });

        public ValueTask<FlightDeal> RemoveFlightDealByIdAsync(Guid driverId) =>
        TryCatch(async () =>
        {
            ValidateFlightDealId(driverId);

            FlightDeal maybeFlightDeal =
                await this.storageBroker.SelectFlightDealByIdAsync(driverId);

            ValidateStorageFlightDeal(maybeFlightDeal, driverId);

            return await this.storageBroker.DeleteFlightDealAsync(maybeFlightDeal);
        });

    }
}
