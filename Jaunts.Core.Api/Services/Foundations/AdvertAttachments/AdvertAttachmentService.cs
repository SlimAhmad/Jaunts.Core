// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments;

namespace Jaunts.Core.Api.Services.Foundations.AdvertAttachments
{
    public partial class AdvertAttachmentService : IAdvertAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public AdvertAttachmentService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<AdvertAttachment> AddAdvertAttachmentAsync(AdvertAttachment advertAttachment) =>
            TryCatch(async () =>
        {
            ValidateAdvertAttachmentOnCreate(advertAttachment);

            return await this.storageBroker.InsertAdvertAttachmentAsync(advertAttachment);
        });

        public IQueryable<AdvertAttachment> RetrieveAllAdvertAttachments() =>
        TryCatch(() => this.storageBroker.SelectAllAdvertAttachments());

        public ValueTask<AdvertAttachment> RetrieveAdvertAttachmentByIdAsync
            (Guid packageId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateAdvertAttachmentIdIsNull(packageId, attachmentId);

            AdvertAttachment maybeAdvertAttachment =
               await this.storageBroker.SelectAdvertAttachmentByIdAsync(packageId, attachmentId);

            ValidateStorageAdvertAttachment(maybeAdvertAttachment, packageId, attachmentId);

            return maybeAdvertAttachment;
        });

        public ValueTask<AdvertAttachment> RemoveAdvertAttachmentByIdAsync(Guid packageId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateAdvertAttachmentIdIsNull(packageId, attachmentId);

            AdvertAttachment mayBeAdvertAttachment =
               await this.storageBroker.SelectAdvertAttachmentByIdAsync(packageId, attachmentId);

            ValidateStorageAdvertAttachment(mayBeAdvertAttachment, packageId, attachmentId);

            return await this.storageBroker.DeleteAdvertAttachmentAsync(mayBeAdvertAttachment);
        });
    }
}
