// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments;

namespace Jaunts.Core.Api.Services.Foundations.ProviderAttachments
{
    public partial class ProviderAttachmentService : IProviderAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ProviderAttachmentService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<ProviderAttachment> AddProviderAttachmentAsync(ProviderAttachment PackageAttachment) =>
            TryCatch(async () =>
        {
            ValidateProviderAttachmentOnCreate(PackageAttachment);

            return await this.storageBroker.InsertProviderAttachmentAsync(PackageAttachment);
        });

        public IQueryable<ProviderAttachment> RetrieveAllProviderAttachments() =>
        TryCatch(() => this.storageBroker.SelectAllProviderAttachments());

        public ValueTask<ProviderAttachment> RetrieveProviderAttachmentByIdAsync
            (Guid packageId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateProviderAttachmentIdIsNull(packageId, attachmentId);

            ProviderAttachment maybeProviderAttachment =
               await this.storageBroker.SelectProviderAttachmentByIdAsync(packageId, attachmentId);

            ValidateStorageProviderAttachment(maybeProviderAttachment, packageId, attachmentId);

            return maybeProviderAttachment;
        });

        public ValueTask<ProviderAttachment> RemoveProviderAttachmentByIdAsync(Guid packageId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateProviderAttachmentIdIsNull(packageId, attachmentId);

            ProviderAttachment mayBeProviderAttachment =
               await this.storageBroker.SelectProviderAttachmentByIdAsync(packageId, attachmentId);

            ValidateStorageProviderAttachment(mayBeProviderAttachment, packageId, attachmentId);

            return await this.storageBroker.DeleteProviderAttachmentAsync(mayBeProviderAttachment);
        });
    }
}
