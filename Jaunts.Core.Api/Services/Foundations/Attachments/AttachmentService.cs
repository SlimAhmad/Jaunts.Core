// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments;

namespace Jaunts.Core.Api.Services.Foundations.Attachments
{
    public partial class AttachmentService : IAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public AttachmentService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Attachment> AddAttachmentAsync(Attachment attachment) =>
        TryCatch(async () =>
        {
            ValidateAttachmentOnCreate(attachment);

            return await this.storageBroker.InsertAttachmentAsync(attachment);
        });

        public IQueryable<Attachment> RetrieveAllAttachments() =>
        TryCatch(() => this.storageBroker.SelectAllAttachments());

        public ValueTask<Attachment> RetrieveAttachmentByIdAsync(Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateAttachmentId(attachmentId);
            Attachment storageAttachment = await this.storageBroker.SelectAttachmentByIdAsync(attachmentId);
            ValidateStorageAttachment(storageAttachment, attachmentId);

            return storageAttachment;
        });

        public ValueTask<Attachment> ModifyAttachmentAsync(Attachment attachment) =>
        TryCatch(async () =>
        {
            ValidateAttachmentOnModify(attachment);
            Attachment maybeAttachment = await this.storageBroker.SelectAttachmentByIdAsync(attachment.Id);
            ValidateStorageAttachment(maybeAttachment, attachment.Id);
            ValidateAgainstStorageAttachmentOnModify(inputAttachment: attachment, storageAttachment: maybeAttachment);

            return await this.storageBroker.UpdateAttachmentAsync(attachment);
        });

        public ValueTask<Attachment> RemoveAttachmentByIdAsync(Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateAttachmentId(attachmentId);
            Attachment maybeAttachment = await this.storageBroker.SelectAttachmentByIdAsync(attachmentId);
            ValidateStorageAttachment(maybeAttachment, attachmentId);

            return await this.storageBroker.DeleteAttachmentAsync(maybeAttachment);
        });
    }
}