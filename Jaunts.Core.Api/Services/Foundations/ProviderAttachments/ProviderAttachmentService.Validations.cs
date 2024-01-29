// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments.Exceptions;
using Termii.Core.Models.Services.Foundations.Termii.Tokens;

namespace Jaunts.Core.Api.Services.Foundations.ProviderAttachments
{
    public partial class ProviderAttachmentService
    {
        private static void ValidateProviderAttachmentOnCreate(ProviderAttachment PackageAttachment)
        {
            ValidateProviderAttachmentIsNull(PackageAttachment);
            ValidateProviderAttachmentIdIsNull(PackageAttachment.ProviderId, PackageAttachment.AttachmentId);
        }

        private static void ValidateProviderAttachmentIsNull(ProviderAttachment ProviderAttachment)
        {
            if (ProviderAttachment is null)
            {
                throw new NullProviderAttachmentException();
            }
        }


        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void ValidateProviderAttachmentId(Guid providerId)
        {
            Validate((Rule: IsInvalid(providerId), Parameter: nameof(ProviderAttachment.ProviderId)));
        }
        private static void ValidateAttachmentId(Guid attachmentId)
        {
            Validate((Rule: IsInvalid(attachmentId), Parameter: nameof(ProviderAttachment.AttachmentId)));
        }

        private static void ValidateProviderAttachmentIdIsNull(Guid providerId, Guid attachmentId)
        {
            ValidateAttachmentId(attachmentId);
            ValidateProviderAttachmentId(providerId);
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

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidProviderAttachmentException = new InvalidProviderAttachmentException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidProviderAttachmentException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidProviderAttachmentException.ThrowIfContainsErrors();
        }

    }
}
