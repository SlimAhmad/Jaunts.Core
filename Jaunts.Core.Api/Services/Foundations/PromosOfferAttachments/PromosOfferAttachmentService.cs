// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments;

namespace Jaunts.Core.Api.Services.Foundations.PromosOfferAttachments
{
    public partial class PromosOfferAttachmentService : IPromosOfferAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public PromosOfferAttachmentService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<PromosOfferAttachment> AddPromosOfferAttachmentAsync(PromosOfferAttachment promosOfferAttachment) =>
            TryCatch(async () =>
        {
            ValidatePromosOfferAttachmentOnCreate(promosOfferAttachment);

            return await this.storageBroker.InsertPromosOfferAttachmentAsync(promosOfferAttachment);
        });

        public IQueryable<PromosOfferAttachment> RetrieveAllPromosOfferAttachments() =>
        TryCatch(() => this.storageBroker.SelectAllPromosOfferAttachments());

        public ValueTask<PromosOfferAttachment> RetrievePromosOfferAttachmentByIdAsync
            (Guid packageId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidatePromosOfferAttachmentIdIsNull(packageId, attachmentId);

            PromosOfferAttachment maybePromosOfferAttachment =
               await this.storageBroker.SelectPromosOfferAttachmentByIdAsync(packageId, attachmentId);

            ValidateStoragePromosOfferAttachment(maybePromosOfferAttachment, packageId, attachmentId);

            return maybePromosOfferAttachment;
        });

        public ValueTask<PromosOfferAttachment> RemovePromosOfferAttachmentByIdAsync(Guid packageId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidatePromosOfferAttachmentIdIsNull(packageId, attachmentId);

            PromosOfferAttachment mayBePromosOfferAttachment =
               await this.storageBroker.SelectPromosOfferAttachmentByIdAsync(packageId, attachmentId);

            ValidateStoragePromosOfferAttachment(mayBePromosOfferAttachment, packageId, attachmentId);

            return await this.storageBroker.DeletePromosOfferAttachmentAsync(mayBePromosOfferAttachment);
        });
    }
}
