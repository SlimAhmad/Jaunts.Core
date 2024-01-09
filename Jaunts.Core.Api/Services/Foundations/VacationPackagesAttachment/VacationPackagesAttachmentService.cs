// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.VacationPackagesAttachments;

namespace Jaunts.Core.Api.Services.Foundations.VacationPackagesAttachments
{
    public partial class VacationPackagesAttachmentService : IVacationPackagesAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public VacationPackagesAttachmentService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<VacationPackagesAttachment> AddVacationPackageAttachmentAsync(VacationPackagesAttachment vacationPackagesAttachment) =>
            TryCatch(async () =>
        {
            ValidateVacationPackagesAttachmentOnCreate(vacationPackagesAttachment);

            return await this.storageBroker.InsertVacationPackagesAttachmentAsync(vacationPackagesAttachment);
        });

        public IQueryable<VacationPackagesAttachment> RetrieveAllVacationPackageAttachments() =>
        TryCatch(() => this.storageBroker.SelectAllVacationPackagesAttachments());

        public ValueTask<VacationPackagesAttachment> RetrieveVacationPackageAttachmentByIdAsync
            (Guid packageId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateVacationPackagesAttachmentIdIsNull(packageId, attachmentId);

            VacationPackagesAttachment maybeVacationPackagesAttachment =
               await this.storageBroker.SelectVacationPackagesAttachmentByIdAsync(packageId, attachmentId);

            ValidateStorageVacationPackagesAttachment(maybeVacationPackagesAttachment, packageId, attachmentId);

            return maybeVacationPackagesAttachment;
        });

        public ValueTask<VacationPackagesAttachment> RemoveVacationPackageAttachmentByIdAsync(Guid packageId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateVacationPackagesAttachmentIdIsNull(packageId, attachmentId);

            VacationPackagesAttachment mayBeVacationPackagesAttachment =
               await this.storageBroker.SelectVacationPackagesAttachmentByIdAsync(packageId, attachmentId);

            ValidateStorageVacationPackagesAttachment(mayBeVacationPackagesAttachment, packageId, attachmentId);

            return await this.storageBroker.DeleteVacationPackagesAttachmentAsync(mayBeVacationPackagesAttachment);
        });
    }
}
