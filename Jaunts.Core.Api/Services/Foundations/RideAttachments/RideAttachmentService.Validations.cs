// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.RideAttachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.RideAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.RideAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.RideAttachments.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.RideAttachments
{
    public partial class RideAttachmentService
    {
        private static void ValidateRideAttachmentOnCreate(RideAttachment rideAttachment)
        {
            ValidateRideAttachmentIsNull(rideAttachment);
            ValidateRideAttachmentIdIsNull(rideAttachment.RideId, rideAttachment.AttachmentId);
        }

        private static void ValidateRideAttachmentIsNull(RideAttachment RideAttachment)
        {
            if (RideAttachment is null)
            {
                throw new NullRideAttachmentException();
            }
        }


        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void ValidateRideAttachmentId(Guid ProviderDirectorId)
        {
            Validate((Rule: IsInvalid(ProviderDirectorId), Parameter: nameof(RideAttachment.RideId)));
        }
        private static void ValidateAttachmentId(Guid attachmentId)
        {
            Validate((Rule: IsInvalid(attachmentId), Parameter: nameof(RideAttachment.AttachmentId)));
        }

        private static void ValidateRideAttachmentIdIsNull(Guid ProviderDirectorId, Guid attachmentId)
        {
            ValidateAttachmentId(attachmentId);
            ValidateRideAttachmentId(ProviderDirectorId);
        }

        private static void ValidateStorageRideAttachment(
            RideAttachment storageRideAttachment,
            Guid ProviderDirectorId, Guid attachmentId)
        {
            if (storageRideAttachment is null)
            {
                throw new NotFoundRideAttachmentException(ProviderDirectorId, attachmentId);
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidRideAttachmentException = new InvalidRideAttachmentException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidRideAttachmentException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidRideAttachmentException.ThrowIfContainsErrors();
        }
    }
}
