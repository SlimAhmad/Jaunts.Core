// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.PromosOfferAttachments
{
    public partial class PromosOfferAttachmentService
    {
        private static void ValidatePromosOfferAttachmentOnCreate(PromosOfferAttachment PromosOfferAttachment)
        {
            ValidatePromosOfferAttachmentIsNull(PromosOfferAttachment);
            ValidatePromosOfferAttachmentIdIsNull(PromosOfferAttachment.PromosOfferId, PromosOfferAttachment.AttachmentId);
        }

        private static void ValidatePromosOfferAttachmentIsNull(PromosOfferAttachment PromosOfferAttachment)
        {
            if (PromosOfferAttachment is null)
            {
                throw new NullPromosOfferAttachmentException();
            }
        }


        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void ValidatePromosOfferAttachmentId(Guid PromosOfferId)
        {
            Validate((Rule: IsInvalid(PromosOfferId), Parameter: nameof(PromosOfferAttachment.PromosOfferId)));
        }
        private static void ValidateAttachmentId(Guid attachmentId)
        {
            Validate((Rule: IsInvalid(attachmentId), Parameter: nameof(PromosOfferAttachment.AttachmentId)));
        }

        private static void ValidatePromosOfferAttachmentIdIsNull(Guid PromosOfferId, Guid attachmentId)
        {
            ValidateAttachmentId(attachmentId);
            ValidatePromosOfferAttachmentId(PromosOfferId);
        }

        private static void ValidateStoragePromosOfferAttachment(
            PromosOfferAttachment storagePromosOfferAttachment,
            Guid PromosOfferId, Guid attachmentId)
        {
            if (storagePromosOfferAttachment is null)
            {
                throw new NotFoundPromosOfferAttachmentException(PromosOfferId, attachmentId);
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidPromosOfferAttachmentException = new InvalidPromosOfferAttachmentException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidPromosOfferAttachmentException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidPromosOfferAttachmentException.ThrowIfContainsErrors();
        }

    }
}
