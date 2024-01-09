// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.VacationPackagesAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.VacationPackagesAttachments.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.VacationPackagesAttachments
{
    public partial class VacationPackagesAttachmentService
    {
        private static void ValidateVacationPackagesAttachmentOnCreate(VacationPackagesAttachment vacationPackagesAttachment)
        {
            ValidateVacationPackagesAttachmentIsNull(vacationPackagesAttachment);
            ValidateVacationPackagesAttachmentIdIsNull(vacationPackagesAttachment.VacationPackagesId, vacationPackagesAttachment.AttachmentId);
        }

        private static void ValidateVacationPackagesAttachmentIsNull(VacationPackagesAttachment VacationPackagesAttachment)
        {
            if (VacationPackagesAttachment is null)
            {
                throw new NullVacationPackagesAttachmentException();
            }
        }

        private static void ValidateVacationPackagesAttachmentIdIsNull(Guid packageId, Guid attachmentId)
        {
            if (packageId == default)
            {
                throw new InvalidVacationPackagesAttachmentException(
                    parameterName: nameof(VacationPackagesAttachment.VacationPackagesId),
                    parameterValue: packageId);
            }
            else if (attachmentId == default)
            {
                throw new InvalidVacationPackagesAttachmentException(
                    parameterName: nameof(VacationPackagesAttachment.AttachmentId),
                    parameterValue: attachmentId);
            }
        }

        private static void ValidateStorageVacationPackagesAttachment(
            VacationPackagesAttachment storageVacationPackageAttachment,
            Guid packageId, Guid attachmentId)
        {
            if (storageVacationPackageAttachment is null)
            {
                throw new NotFoundVacationPackagesAttachmentException(packageId, attachmentId);
            }
        }
    }
}
