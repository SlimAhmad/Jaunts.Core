// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Attachments;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments;

namespace Jaunts.Core.Api.Services.Foundations.Attachments
{
    public partial class AttachmentService : IAttachmentService
    {
        private void ValidateAttachmentOnCreate(Attachment attachment)
        {
            ValidateAttachmentIsNull(attachment);
            Validate(
              (Rule: IsInvalid(attachment.Id), Parameter: nameof(Attachment.Id)),
              (Rule: IsInvalid(attachment.Description), Parameter: nameof(Attachment.Description)),
              (Rule: IsInvalid(attachment.Extension), Parameter: nameof(Attachment.Extension)),
              (Rule: IsInvalid(attachment.ExternalUrl), Parameter: nameof(Attachment.ExternalUrl)),
              (Rule: IsInvalid(attachment.Label), Parameter: nameof(Attachment.Label)),
              (Rule: IsInvalid(attachment.ContentType), Parameter: nameof(Attachment.ContentType)),
              (Rule: IsInvalid(attachment.Status), Parameter: nameof(Attachment.Status)),
              (Rule: IsInvalid(attachment.CreatedBy), Parameter: nameof(Attachment.CreatedBy)),
              (Rule: IsInvalid(attachment.UpdatedBy), Parameter: nameof(Attachment.UpdatedBy)),
              (Rule: IsInvalid(attachment.CreatedDate), Parameter: nameof(Attachment.CreatedDate)),
              (Rule: IsInvalid(attachment.UpdatedDate), Parameter: nameof(Attachment.UpdatedDate)),
              (Rule: IsNotRecent(attachment.CreatedDate), Parameter: nameof(Attachment.CreatedDate)),

              (Rule: IsNotSame(firstId: attachment.UpdatedBy,
                  secondId: attachment.CreatedBy,
                  secondIdName: nameof(Attachment.CreatedBy)),
            Parameter: nameof(Attachment.UpdatedBy)),
            (Rule: IsNotSame(firstDate: attachment.UpdatedDate,
                  secondDate: attachment.CreatedDate,
                  secondDateName: nameof(Attachment.CreatedDate)),
                  Parameter: nameof(Attachment.UpdatedDate)));

        }

        private void ValidateAttachmentOnModify(Attachment attachment)
        {
            ValidateAttachmentIsNull(attachment);

            Validate(
                (Rule: IsInvalid(attachment.Id), Parameter: nameof(Attachment.Id)),
                (Rule: IsInvalid(attachment.Description), Parameter: nameof(Attachment.Description)),
                (Rule: IsInvalid(attachment.Extension), Parameter: nameof(Attachment.Extension)),
                (Rule: IsInvalid(attachment.ExternalUrl), Parameter: nameof(Attachment.ExternalUrl)),
                (Rule: IsInvalid(attachment.Label), Parameter: nameof(Attachment.Label)),
                (Rule: IsInvalid(attachment.ContentType), Parameter: nameof(Attachment.ContentType)),
                (Rule: IsInvalid(attachment.Status), Parameter: nameof(Attachment.Status)),
                (Rule: IsInvalid(attachment.CreatedBy), Parameter: nameof(Attachment.CreatedBy)),
                (Rule: IsInvalid(attachment.UpdatedBy), Parameter: nameof(Attachment.UpdatedBy)),
                (Rule: IsInvalid(attachment.CreatedDate), Parameter: nameof(Attachment.CreatedDate)),
                (Rule: IsInvalid(attachment.UpdatedDate), Parameter: nameof(Attachment.UpdatedDate)),
                (Rule: IsNotRecent(attachment.UpdatedDate), Parameter: nameof(Attachment.UpdatedDate)),

          (Rule: IsSame(
                firstDate: attachment.UpdatedDate,
                secondDate: attachment.CreatedDate,
                secondDateName: nameof(Attachment.CreatedDate)),
                Parameter: nameof(Attachment.UpdatedDate)));
        }

        private static void ValidateStorageAttachment(Attachment storageAttachment, Guid attachmentId)
        {
            if (storageAttachment is null)
            {
                throw new NotFoundAttachmentException(attachmentId);
            }
        }

        private static void ValidateAgainstStorageAttachmentOnModify(
            Attachment inputAttachment,
            Attachment storageAttachment)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputAttachment.CreatedDate,
                    secondDate: storageAttachment.CreatedDate,
                    secondDateName: nameof(Attachment.CreatedDate)),
                    Parameter: nameof(Attachment.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputAttachment.UpdatedDate,
                    secondDate: storageAttachment.UpdatedDate,
                    secondDateName: nameof(Attachment.UpdatedDate)),
                    Parameter: nameof(Attachment.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: inputAttachment.CreatedBy,
                    secondId: storageAttachment.CreatedBy,
                    secondIdName: nameof(Attachment.CreatedBy)),
                    Parameter: nameof(Attachment.CreatedBy))
            );
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsNotSame(
            Guid firstId,
            Guid secondId,
            string secondIdName) => new
            {
                Condition = firstId != secondId,
                Message = $"Id is not the same as {secondIdName}"
            };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private static dynamic IsInvalid(AttachmentStatus status) => new
        {
            Condition = Enum.IsDefined(status) is false,
            Message = "Value is not recognized"
        };
        private dynamic IsNotRecent(DateTimeOffset dateTimeOffset) => new
        {
            Condition = IsDateNotRecent(dateTimeOffset),
            Message = "Date is not recent"
        };


        private static void ValidateAttachmentId(Guid attachmentId)
        {
            Validate((Rule: IsInvalid(attachmentId), Parameter: nameof(Attachment.Id)));
        }

        private static void ValidateAttachmentIsNull(Attachment attachment)
        {
            if (attachment is null)
            {
                throw new NullAttachmentException();
            }
        }

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTime();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidAttachmentException = new InvalidAttachmentException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidAttachmentException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidAttachmentException.ThrowIfContainsErrors();
        }
    }
}
