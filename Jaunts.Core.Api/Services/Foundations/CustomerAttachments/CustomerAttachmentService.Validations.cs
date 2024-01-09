// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.CustomerAttachments
{
    public partial class CustomerAttachmentService
    {
        private static void ValidateCustomerAttachmentOnCreate(CustomerAttachment vacationPackagesAttachment)
        {
            ValidateCustomerAttachmentIsNull(vacationPackagesAttachment);
            ValidateCustomerAttachmentIdIsNull(vacationPackagesAttachment.CustomerId, vacationPackagesAttachment.AttachmentId);
        }

        private static void ValidateCustomerAttachmentIsNull(CustomerAttachment customerAttachment)
        {
            if (customerAttachment is null)
            {
                throw new NullCustomerAttachmentException();
            }
        }

        private static void ValidateCustomerAttachmentIdIsNull(Guid customerId, Guid attachmentId)
        {
            if (customerId == default)
            {
                throw new InvalidCustomerAttachmentException(
                    parameterName: nameof(CustomerAttachment.CustomerId),
                    parameterValue: customerId);
            }
            else if (attachmentId == default)
            {
                throw new InvalidCustomerAttachmentException(
                    parameterName: nameof(CustomerAttachment.AttachmentId),
                    parameterValue: attachmentId);
            }
        }

        private static void ValidateStorageCustomerAttachment(
            CustomerAttachment storageCustomerAttachment,
            Guid customerId, Guid attachmentId)
        {
            if (storageCustomerAttachment is null)
            {
                throw new NotFoundCustomerAttachmentException(customerId, attachmentId);
            }
        }
    }
}
