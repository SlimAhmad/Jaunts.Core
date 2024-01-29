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
        private static void ValidateProvidersDirectorAttachmentOnCreate(ProvidersDirectorAttachment providersDirectorAttachment)
        {
            ValidateProvidersDirectorAttachmentIsNull(providersDirectorAttachment);
            ValidateProvidersDirectorAttachmentIdIsNull(providersDirectorAttachment.ProviderDirectorId, providersDirectorAttachment.AttachmentId);
        }

        private static void ValidateProvidersDirectorAttachmentIsNull(ProvidersDirectorAttachment ProvidersDirectorAttachment)
        {
            if (ProvidersDirectorAttachment is null)
            {
                throw new NullProvidersDirectorAttachmentException();
            }
        }


        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void ValidateProvidersDirectorAttachmentId(Guid ProviderDirectorId)
        {
            Validate((Rule: IsInvalid(ProviderDirectorId), Parameter: nameof(ProvidersDirectorAttachment.ProviderDirectorId)));
        }
        private static void ValidateAttachmentId(Guid attachmentId)
        {
            Validate((Rule: IsInvalid(attachmentId), Parameter: nameof(ProvidersDirectorAttachment.AttachmentId)));
        }

        private static void ValidateProvidersDirectorAttachmentIdIsNull(Guid ProviderDirectorId, Guid attachmentId)
        {
            ValidateAttachmentId(attachmentId);
            ValidateProvidersDirectorAttachmentId(ProviderDirectorId);
        }

        private static void ValidateStorageProvidersDirectorAttachment(
            ProvidersDirectorAttachment storageProvidersDirectorAttachment,
            Guid ProviderDirectorId, Guid attachmentId)
        {
            if (storageProvidersDirectorAttachment is null)
            {
                throw new NotFoundProvidersDirectorAttachmentException(ProviderDirectorId, attachmentId);
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidProvidersDirectorAttachmentException = new InvalidProvidersDirectorAttachmentException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidProvidersDirectorAttachmentException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidProvidersDirectorAttachmentException.ThrowIfContainsErrors();
        }
    }
}
