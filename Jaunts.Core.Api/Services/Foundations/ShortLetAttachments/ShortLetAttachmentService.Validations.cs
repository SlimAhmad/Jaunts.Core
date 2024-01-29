// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.ShortLetAttachments
{
    public partial class ShortLetAttachmentService
    {
        private static void ValidateShortLetAttachmentOnCreate(ShortLetAttachment shortLetAttachment)
        {
            ValidateShortLetAttachmentIsNull(shortLetAttachment);
            ValidateShortLetAttachmentIdIsNull(shortLetAttachment.ShortLetId, shortLetAttachment.AttachmentId);
        }

        private static void ValidateShortLetAttachmentIsNull(ShortLetAttachment ShortLetAttachment)
        {
            if (ShortLetAttachment is null)
            {
                throw new NullShortLetAttachmentException();
            }
        }


        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void ValidateShortLetAttachmentId(Guid ShortLetId)
        {
            Validate((Rule: IsInvalid(ShortLetId), Parameter: nameof(ShortLetAttachment.ShortLetId)));
        }
        private static void ValidateAttachmentId(Guid attachmentId)
        {
            Validate((Rule: IsInvalid(attachmentId), Parameter: nameof(ShortLetAttachment.AttachmentId)));
        }

        private static void ValidateShortLetAttachmentIdIsNull(Guid ShortLetId, Guid attachmentId)
        {
            ValidateAttachmentId(attachmentId);
            ValidateShortLetAttachmentId(ShortLetId);
        }

        private static void ValidateStorageShortLetAttachment(
            ShortLetAttachment storageShortLetAttachment,
            Guid ShortLetId, Guid attachmentId)
        {
            if (storageShortLetAttachment is null)
            {
                throw new NotFoundShortLetAttachmentException(ShortLetId, attachmentId);
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidShortLetAttachmentException = new InvalidShortLetAttachmentException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidShortLetAttachmentException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidShortLetAttachmentException.ThrowIfContainsErrors();
        }
    }
}
