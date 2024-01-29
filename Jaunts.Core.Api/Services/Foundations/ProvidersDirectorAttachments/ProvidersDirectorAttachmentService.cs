// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments;

namespace Jaunts.Core.Api.Services.Foundations.ProvidersDirectorAttachments
{
    public partial class ProvidersDirectorAttachmentService : IProvidersDirectorAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ProvidersDirectorAttachmentService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<ProvidersDirectorAttachment> AddProvidersDirectorAttachmentAsync(ProvidersDirectorAttachment PackageAttachment) =>
            TryCatch(async () =>
        {
            ValidateProvidersDirectorAttachmentOnCreate(PackageAttachment);

            return await this.storageBroker.InsertProvidersDirectorAttachmentAsync(PackageAttachment);
        });

        public IQueryable<ProvidersDirectorAttachment> RetrieveAllProvidersDirectorAttachments() =>
        TryCatch(() => this.storageBroker.SelectAllProvidersDirectorAttachments());

        public ValueTask<ProvidersDirectorAttachment> RetrieveProvidersDirectorAttachmentByIdAsync
            (Guid packageId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateProvidersDirectorAttachmentIdIsNull(packageId, attachmentId);

            ProvidersDirectorAttachment maybeProvidersDirectorAttachment =
               await this.storageBroker.SelectProvidersDirectorAttachmentByIdAsync(packageId, attachmentId);

            ValidateStorageProvidersDirectorAttachment(maybeProvidersDirectorAttachment, packageId, attachmentId);

            return maybeProvidersDirectorAttachment;
        });

        public ValueTask<ProvidersDirectorAttachment> RemoveProvidersDirectorAttachmentByIdAsync(Guid packageId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateProvidersDirectorAttachmentIdIsNull(packageId, attachmentId);

            ProvidersDirectorAttachment mayBeProvidersDirectorAttachment =
               await this.storageBroker.SelectProvidersDirectorAttachmentByIdAsync(packageId, attachmentId);

            ValidateStorageProvidersDirectorAttachment(mayBeProvidersDirectorAttachment, packageId, attachmentId);

            return await this.storageBroker.DeleteProvidersDirectorAttachmentAsync(mayBeProvidersDirectorAttachment);
        });
    }
}
