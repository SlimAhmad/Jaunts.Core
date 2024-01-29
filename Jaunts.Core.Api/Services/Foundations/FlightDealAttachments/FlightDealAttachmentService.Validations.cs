// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.FlightDealAttachments
{
    public partial class FlightDealAttachmentService
    {
        private static void ValidateFlightDealAttachmentOnCreate(FlightDealAttachment FlightDealAttachment)
        {
            ValidateFlightDealAttachmentIsNull(FlightDealAttachment);
            ValidateFlightDealAttachmentIdIsNull(FlightDealAttachment.FlightDealId, FlightDealAttachment.AttachmentId);
        }

        private static void ValidateFlightDealAttachmentIsNull(FlightDealAttachment FlightDealAttachment)
        {
            if (FlightDealAttachment is null)
            {
                throw new NullFlightDealAttachmentException();
            }
        }


        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void ValidateFlightDealAttachmentId(Guid FlightDealId)
        {
            Validate((Rule: IsInvalid(FlightDealId), Parameter: nameof(FlightDealAttachment.FlightDealId)));
        }
        private static void ValidateAttachmentId(Guid attachmentId)
        {
            Validate((Rule: IsInvalid(attachmentId), Parameter: nameof(FlightDealAttachment.AttachmentId)));
        }

        private static void ValidateFlightDealAttachmentIdIsNull(Guid FlightDealId, Guid attachmentId)
        {
            ValidateAttachmentId(attachmentId);
            ValidateFlightDealAttachmentId(FlightDealId);
        }

        private static void ValidateStorageFlightDealAttachment(
            FlightDealAttachment storageFlightDealAttachment,
            Guid FlightDealId, Guid attachmentId)
        {
            if (storageFlightDealAttachment is null)
            {
                throw new NotFoundFlightDealAttachmentException(FlightDealId, attachmentId);
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidFlightDealAttachmentException = new InvalidFlightDealAttachmentException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidFlightDealAttachmentException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidFlightDealAttachmentException.ThrowIfContainsErrors();
        }

    }
}
