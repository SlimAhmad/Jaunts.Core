// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments;

namespace Jaunts.Core.Api.Services.Foundations.DriverAttachments
{
    public partial class DriverAttachmentService : IDriverAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public DriverAttachmentService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<DriverAttachment> AddDriverAttachmentAsync(DriverAttachment driverAttachment) =>
            TryCatch(async () =>
        {
            ValidateDriverAttachmentOnCreate(driverAttachment);

            return await this.storageBroker.InsertDriverAttachmentAsync(driverAttachment);
        });

        public IQueryable<DriverAttachment> RetrieveAllDriverAttachments() =>
        TryCatch(() => this.storageBroker.SelectAllDriverAttachments());

        public ValueTask<DriverAttachment> RetrieveDriverAttachmentByIdAsync
            (Guid packageId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateDriverAttachmentIdIsNull(packageId, attachmentId);

            DriverAttachment maybeDriverAttachment =
               await this.storageBroker.SelectDriverAttachmentByIdAsync(packageId, attachmentId);

            ValidateStorageDriverAttachment(maybeDriverAttachment, packageId, attachmentId);

            return maybeDriverAttachment;
        });

        public ValueTask<DriverAttachment> RemoveDriverAttachmentByIdAsync(Guid packageId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateDriverAttachmentIdIsNull(packageId, attachmentId);

            DriverAttachment mayBeDriverAttachment =
               await this.storageBroker.SelectDriverAttachmentByIdAsync(packageId, attachmentId);

            ValidateStorageDriverAttachment(mayBeDriverAttachment, packageId, attachmentId);

            return await this.storageBroker.DeleteDriverAttachmentAsync(mayBeDriverAttachment);
        });
    }
}
