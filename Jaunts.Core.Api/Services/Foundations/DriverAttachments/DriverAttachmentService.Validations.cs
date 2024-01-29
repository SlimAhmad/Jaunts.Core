// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.DriverAttachments
{
    public partial class DriverAttachmentService
    {
        private static void ValidateDriverAttachmentOnCreate(DriverAttachment driverAttachment)
        {
            ValidateDriverAttachmentIsNull(driverAttachment);
            ValidateDriverAttachmentIdIsNull(driverAttachment.DriverId, driverAttachment.AttachmentId);
        }

        private static void ValidateDriverAttachmentIsNull(DriverAttachment DriverAttachment)
        {
            if (DriverAttachment is null)
            {
                throw new NullDriverAttachmentException();
            }
        }


        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void ValidateDriverAttachmentId(Guid DriverId)
        {
            Validate((Rule: IsInvalid(DriverId), Parameter: nameof(DriverAttachment.DriverId)));
        }
        private static void ValidateAttachmentId(Guid attachmentId)
        {
            Validate((Rule: IsInvalid(attachmentId), Parameter: nameof(DriverAttachment.AttachmentId)));
        }

        private static void ValidateDriverAttachmentIdIsNull(Guid DriverId, Guid attachmentId)
        {
            ValidateAttachmentId(attachmentId);
            ValidateDriverAttachmentId(DriverId);
        }

        private static void ValidateStorageDriverAttachment(
            DriverAttachment storageDriverAttachment,
            Guid DriverId, Guid attachmentId)
        {
            if (storageDriverAttachment is null)
            {
                throw new NotFoundDriverAttachmentException(DriverId, attachmentId);
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidDriverAttachmentException = new InvalidDriverAttachmentException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDriverAttachmentException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDriverAttachmentException.ThrowIfContainsErrors();
        }

    }
}
