// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.ProviderAttachments
{
    public partial class ProviderAttachmentService
    {
        private static void ValidateProviderAttachmentOnCreate(ProviderAttachment vacationPackagesAttachment)
        {
            ValidateProviderAttachmentIsNull(vacationPackagesAttachment);
            ValidateProviderAttachmentIdIsNull(vacationPackagesAttachment.ProviderId, vacationPackagesAttachment.AttachmentId);
        }

        private static void ValidateProviderAttachmentIsNull(ProviderAttachment ProviderAttachment)
        {
            if (ProviderAttachment is null)
            {
                throw new NullProviderAttachmentException();
            }
        }

        private static void ValidateProviderAttachmentIdIsNull(Guid providerId, Guid attachmentId)
        {
            if (providerId == default)
            {
                throw new InvalidProviderAttachmentException(
                    parameterName: nameof(ProviderAttachment.ProviderId),
                    parameterValue: providerId);
            }
            else if (attachmentId == default)
            {
                throw new InvalidProviderAttachmentException(
                    parameterName: nameof(ProviderAttachment.AttachmentId),
                    parameterValue: attachmentId);
            }
        }

        private static void ValidateStorageProviderAttachment(
            ProviderAttachment storageProviderAttachment,
            Guid providerId, Guid attachmentId)
        {
            if (storageProviderAttachment is null)
            {
                throw new NotFoundProviderAttachmentException(providerId, attachmentId);
            }
        }
    }
}
