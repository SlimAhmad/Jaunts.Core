// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments;

namespace Jaunts.Core.Api.Services.Foundations.PackagesAttachments
{
    public partial class PackageAttachmentService : IPackageAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public PackageAttachmentService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<PackageAttachment> AddPackageAttachmentAsync(PackageAttachment PackageAttachment) =>
            TryCatch(async () =>
        {
            ValidatePackageAttachmentOnCreate(PackageAttachment);

            return await this.storageBroker.InsertPackageAttachmentAsync(PackageAttachment);
        });

        public IQueryable<PackageAttachment> RetrieveAllPackageAttachments() =>
        TryCatch(() => this.storageBroker.SelectAllPackageAttachments());

        public ValueTask<PackageAttachment> RetrievePackageAttachmentByIdAsync
            (Guid packageId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidatePackageAttachmentIdIsNull(packageId, attachmentId);

            PackageAttachment maybePackageAttachment =
               await this.storageBroker.SelectPackageAttachmentByIdAsync(packageId, attachmentId);

            ValidateStoragePackageAttachment(maybePackageAttachment, packageId, attachmentId);

            return maybePackageAttachment;
        });

        public ValueTask<PackageAttachment> RemovePackageAttachmentByIdAsync(Guid packageId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidatePackageAttachmentIdIsNull(packageId, attachmentId);

            PackageAttachment mayBePackageAttachment =
               await this.storageBroker.SelectPackageAttachmentByIdAsync(packageId, attachmentId);

            ValidateStoragePackageAttachment(mayBePackageAttachment, packageId, attachmentId);

            return await this.storageBroker.DeletePackageAttachmentAsync(mayBePackageAttachment);
        });
    }
}
