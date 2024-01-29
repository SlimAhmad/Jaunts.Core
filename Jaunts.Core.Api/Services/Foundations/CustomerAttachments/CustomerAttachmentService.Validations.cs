// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals;
using Jaunts.Core.Api.Models.Services.Foundations.Customers.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Customers;

namespace Jaunts.Core.Api.Services.Foundations.CustomerAttachments
{
    public partial class CustomerAttachmentService
    {
        private static void ValidateCustomerAttachmentOnCreate(CustomerAttachment customerAttachment)
        {
            ValidateCustomerAttachmentIsNull(customerAttachment);
            Validate(
                (Rule: IsInvalid(customerAttachment.CustomerId), Parameter: nameof(CustomerAttachment.CustomerId)),
                (Rule: IsInvalid(customerAttachment.AttachmentId), Parameter: nameof(CustomerAttachment.AttachmentId))
            );
        }

        private static void ValidateCustomerAttachmentIsNull(CustomerAttachment customerAttachment)
        {
            if (customerAttachment is null)
            {
                throw new NullCustomerAttachmentException();
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

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void ValidateCustomerAttachmentIdIsNull(Guid customerId,Guid attachmentId)
        {
            Validate(
                (Rule: IsInvalid(customerId), Parameter: nameof(CustomerAttachment.CustomerId)),
                (Rule: IsInvalid(attachmentId), Parameter: nameof(CustomerAttachment.AttachmentId))
                );
        }

        private static void ValidateStorageCustomer(Customer storageCustomer, Guid flightDealId)
        {
            if (storageCustomer is null)
            {
                throw new NotFoundCustomerException(flightDealId);
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidCustomerAttachmentException = new InvalidCustomerAttachmentException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidCustomerAttachmentException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidCustomerAttachmentException.ThrowIfContainsErrors();
        }
    }
}
