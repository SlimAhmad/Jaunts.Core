// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments;

namespace Jaunts.Core.Api.Services.Foundations.ShortLetAttachments
{
    public partial class ShortLetAttachmentService : IShortLetAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ShortLetAttachmentService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<ShortLetAttachment> AddShortLetAttachmentAsync(ShortLetAttachment shortLetAttachment) =>
            TryCatch(async () =>
        {
            ValidateShortLetAttachmentOnCreate(shortLetAttachment);

            return await this.storageBroker.InsertShortLetAttachmentAsync(shortLetAttachment);
        });

        public IQueryable<ShortLetAttachment> RetrieveAllShortLetAttachments() =>
        TryCatch(() => this.storageBroker.SelectAllShortLetAttachments());

        public ValueTask<ShortLetAttachment> RetrieveShortLetAttachmentByIdAsync
            (Guid packageId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateShortLetAttachmentIdIsNull(packageId, attachmentId);

            ShortLetAttachment maybeShortLetAttachment =
               await this.storageBroker.SelectShortLetAttachmentByIdAsync(packageId, attachmentId);

            ValidateStorageShortLetAttachment(maybeShortLetAttachment, packageId, attachmentId);

            return maybeShortLetAttachment;
        });

        public ValueTask<ShortLetAttachment> RemoveShortLetAttachmentByIdAsync(Guid packageId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateShortLetAttachmentIdIsNull(packageId, attachmentId);

            ShortLetAttachment mayBeShortLetAttachment =
               await this.storageBroker.SelectShortLetAttachmentByIdAsync(packageId, attachmentId);

            ValidateStorageShortLetAttachment(mayBeShortLetAttachment, packageId, attachmentId);

            return await this.storageBroker.DeleteShortLetAttachmentAsync(mayBeShortLetAttachment);
        });
    }
}
