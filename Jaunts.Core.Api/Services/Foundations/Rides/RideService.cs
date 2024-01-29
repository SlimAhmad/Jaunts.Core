// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.Rides;

namespace Jaunts.Core.Api.Services.Foundations.Rides
{
    public partial class RideService : IRideService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public RideService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Ride> CreateRideAsync(Ride Ride) =>
        TryCatch(async () =>
        {
            ValidateRideOnRegister(Ride);

            return await this.storageBroker.InsertRideAsync(Ride);
        });

        public IQueryable<Ride> RetrieveAllRides() =>
        TryCatch(() => this.storageBroker.SelectAllRides());

        public ValueTask<Ride> RetrieveRideByIdAsync(Guid RideId) =>
        TryCatch(async () =>
        {
            ValidateRideId(RideId);
            Ride maybeRide = await this.storageBroker.SelectRideByIdAsync(RideId);
            ValidateStorageRide(maybeRide, RideId);

            return maybeRide;
        });

        public ValueTask<Ride> ModifyRideAsync(Ride Ride) =>
        TryCatch(async () =>
        {
            ValidateRideOnModify(Ride);

            Ride maybeRide =
                await this.storageBroker.SelectRideByIdAsync(Ride.Id);

            ValidateStorageRide(maybeRide, Ride.Id);
            ValidateAgainstStorageRideOnModify(inputRide: Ride, storageRide: maybeRide);

            return await this.storageBroker.UpdateRideAsync(Ride);
        });

        public ValueTask<Ride> RemoveRideByIdAsync(Guid RideId) =>
        TryCatch(async () =>
        {
            ValidateRideId(RideId);

            Ride maybeRide =
                await this.storageBroker.SelectRideByIdAsync(RideId);

            ValidateStorageRide(maybeRide, RideId);

            return await this.storageBroker.DeleteRideAsync(maybeRide);
        });

    }
}
