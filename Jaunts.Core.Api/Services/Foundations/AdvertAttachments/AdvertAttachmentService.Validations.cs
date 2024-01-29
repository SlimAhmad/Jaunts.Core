// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.AdvertAttachments
{
    public partial class AdvertAttachmentService
    {
        private static void ValidateAdvertAttachmentOnCreate(AdvertAttachment AdvertAttachment)
        {
            ValidateAdvertAttachmentIsNull(AdvertAttachment);
            ValidateAdvertAttachmentIdIsNull(AdvertAttachment.AdvertId, AdvertAttachment.AttachmentId);
        }

        private static void ValidateAdvertAttachmentIsNull(AdvertAttachment AdvertAttachment)
        {
            if (AdvertAttachment is null)
            {
                throw new NullAdvertAttachmentException();
            }
        }


        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void ValidateAdvertAttachmentId(Guid AdvertId)
        {
            Validate((Rule: IsInvalid(AdvertId), Parameter: nameof(AdvertAttachment.AdvertId)));
        }
        private static void ValidateAttachmentId(Guid attachmentId)
        {
            Validate((Rule: IsInvalid(attachmentId), Parameter: nameof(AdvertAttachment.AttachmentId)));
        }

        private static void ValidateAdvertAttachmentIdIsNull(Guid AdvertId, Guid attachmentId)
        {
            ValidateAttachmentId(attachmentId);
            ValidateAdvertAttachmentId(AdvertId);
        }

        private static void ValidateStorageAdvertAttachment(
            AdvertAttachment storageAdvertAttachment,
            Guid AdvertId, Guid attachmentId)
        {
            if (storageAdvertAttachment is null)
            {
                throw new NotFoundAdvertAttachmentException(AdvertId, attachmentId);
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidAdvertAttachmentException = new InvalidAdvertAttachmentException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidAdvertAttachmentException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidAdvertAttachmentException.ThrowIfContainsErrors();
        }

    }
}
