// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.Amenities;

namespace Jaunts.Core.Api.Services.Foundations.Amenities
{
    public partial class AmenityService : IAmenityService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public AmenityService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Amenity> CreateAmenityAsync(Amenity amenity) =>
        TryCatch(async () =>
        {
            ValidateAmenityOnCreate(amenity);

            return await this.storageBroker.InsertAmenityAsync(amenity);
        });

        public IQueryable<Amenity> RetrieveAllAmenities() =>
        TryCatch(() => this.storageBroker.SelectAllAmenities());

        public ValueTask<Amenity> RetrieveAmenityByIdAsync(Guid amenityId) =>
        TryCatch(async () =>
        {
            ValidateAmenityId(amenityId);
            Amenity maybeAmenity = await this.storageBroker.SelectAmenityByIdAsync(amenityId);
            ValidateStorageAmenity(maybeAmenity, amenityId);

            return maybeAmenity;
        });

        public ValueTask<Amenity> ModifyAmenityAsync(Amenity amenity) =>
        TryCatch(async () =>
        {
            ValidateAmenityOnModify(amenity);

            Amenity maybeAmenity =
                await this.storageBroker.SelectAmenityByIdAsync(amenity.Id);

            ValidateStorageAmenity(maybeAmenity, amenity.Id);
            ValidateAgainstStorageAmenityOnModify(inputAmenity: amenity, storageAmenity: maybeAmenity);

            return await this.storageBroker.UpdateAmenityAsync(amenity);
        });

        public ValueTask<Amenity> RemoveAmenityByIdAsync(Guid amenityId) =>
        TryCatch(async () =>
        {
            ValidateAmenityId(amenityId);

            Amenity maybeAmenity =
                await this.storageBroker.SelectAmenityByIdAsync(amenityId);

            ValidateStorageAmenity(maybeAmenity, amenityId);

            return await this.storageBroker.DeleteAmenityAsync(maybeAmenity);
        });

    }
}
