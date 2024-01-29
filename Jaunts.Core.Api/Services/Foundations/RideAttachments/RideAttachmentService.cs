// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.RideAttachments;

namespace Jaunts.Core.Api.Services.Foundations.RideAttachments
{
    public partial class RideAttachmentService : IRideAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public RideAttachmentService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<RideAttachment> AddRideAttachmentAsync(RideAttachment PackageAttachment) =>
            TryCatch(async () =>
        {
            ValidateRideAttachmentOnCreate(PackageAttachment);

            return await this.storageBroker.InsertRideAttachmentAsync(PackageAttachment);
        });

        public IQueryable<RideAttachment> RetrieveAllRideAttachments() =>
        TryCatch(() => this.storageBroker.SelectAllRideAttachments());

        public ValueTask<RideAttachment> RetrieveRideAttachmentByIdAsync
            (Guid packageId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateRideAttachmentIdIsNull(packageId, attachmentId);

            RideAttachment maybeRideAttachment =
               await this.storageBroker.SelectRideAttachmentByIdAsync(packageId, attachmentId);

            ValidateStorageRideAttachment(maybeRideAttachment, packageId, attachmentId);

            return maybeRideAttachment;
        });

        public ValueTask<RideAttachment> RemoveRideAttachmentByIdAsync(Guid packageId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateRideAttachmentIdIsNull(packageId, attachmentId);

            RideAttachment mayBeRideAttachment =
               await this.storageBroker.SelectRideAttachmentByIdAsync(packageId, attachmentId);

            ValidateStorageRideAttachment(mayBeRideAttachment, packageId, attachmentId);

            return await this.storageBroker.DeleteRideAttachmentAsync(mayBeRideAttachment);
        });
    }
}
