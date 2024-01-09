// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.ProvidersDirectorAttachments
{
    public partial class ProvidersDirectorAttachmentService
    {
        private static void ValidateProvidersDirectorAttachmentOnCreate(ProvidersDirectorAttachment vacationPackagesAttachment)
        {
            ValidateProvidersDirectorAttachmentIsNull(vacationPackagesAttachment);
            ValidateProvidersDirectorAttachmentIdIsNull(vacationPackagesAttachment.ProviderId, vacationPackagesAttachment.AttachmentId);
        }

        private static void ValidateProvidersDirectorAttachmentIsNull(ProvidersDirectorAttachment ProvidersDirectorAttachment)
        {
            if (ProvidersDirectorAttachment is null)
            {
                throw new NullProvidersDirectorAttachmentException();
            }
        }

        private static void ValidateProvidersDirectorAttachmentIdIsNull(Guid packageId, Guid attachmentId)
        {
            if (packageId == default)
            {
                throw new InvalidProvidersDirectorAttachmentException(
                    parameterName: nameof(ProvidersDirectorAttachment.ProviderId),
                    parameterValue: packageId);
            }
            else if (attachmentId == default)
            {
                throw new InvalidProvidersDirectorAttachmentException(
                    parameterName: nameof(ProvidersDirectorAttachment.AttachmentId),
                    parameterValue: attachmentId);
            }
        }

        private static void ValidateStorageProvidersDirectorAttachment(
            ProvidersDirectorAttachment storageVacationPackageAttachment,
            Guid packageId, Guid attachmentId)
        {
            if (storageVacationPackageAttachment is null)
            {
                throw new NotFoundProvidersDirectorAttachmentException(packageId, attachmentId);
            }
        }
    }
}
