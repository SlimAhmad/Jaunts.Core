// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Customers;
using Jaunts.Core.Api.Models.Services.Foundations.Customers.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.Customers
{
    public partial class CustomerService
    {
        private void ValidateCustomerOnCreate(Customer customer)
        {
            ValidateCustomer(customer);

            Validate(
                (Rule: IsInvalid(customer.Id), Parameter: nameof(Customer.Id)),
                (Rule: IsInvalid(customer.LastName), Parameter: nameof(Customer.LastName)),
                (Rule: IsInvalid(customer.FirstName), Parameter: nameof(Customer.FirstName)),
                (Rule: IsInvalid(customer.MiddleName), Parameter: nameof(Customer.MiddleName)),
                (Rule: IsInvalid(customer.Address), Parameter: nameof(Customer.Address)),
                (Rule: IsInvalid(customer.BirthDate), Parameter: nameof(Customer.BirthDate)),
                (Rule: IsInvalid(customer.UserId), Parameter: nameof(Customer.UserId)),
                (Rule: IsInvalid(customer.CreatedBy), Parameter: nameof(Customer.CreatedBy)),
                (Rule: IsInvalid(customer.UpdatedBy), Parameter: nameof(Customer.UpdatedBy)),
                (Rule: IsInvalid(customer.CreatedDate), Parameter: nameof(Customer.CreatedDate)),
                (Rule: IsInvalid(customer.UpdatedDate), Parameter: nameof(Customer.UpdatedDate)),
                (Rule: IsNotRecent(customer.CreatedDate), Parameter: nameof(Customer.CreatedDate)),

                (Rule: IsNotSame(firstId: customer.UpdatedBy,
                    secondId: customer.CreatedBy,
                    secondIdName: nameof(Customer.CreatedBy)),
                    Parameter: nameof(Customer.UpdatedBy)),

                (Rule: IsNotSame(firstDate: customer.UpdatedDate,
                    secondDate: customer.CreatedDate,
                    secondDateName: nameof(Customer.CreatedDate)),
                    Parameter: nameof(Customer.UpdatedDate))
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

        private dynamic IsNotRecent(DateTimeOffset dateTimeOffset) => new
        {
            Condition = IsDateNotRecent(dateTimeOffset),
            Message = "Date is not recent"
        };

        private static void ValidateCustomerId(Guid customerId)
        {
            Validate((Rule: IsInvalid(customerId), Parameter: nameof(Customer.Id)));
        }

        private static void ValidateStorageCustomer(Customer storageCustomer, Guid customerId)
        {
            if (storageCustomer is null)
            {
                throw new NotFoundCustomerException(customerId);
            }
        }

        private void ValidateCustomerOnModify(Customer customer)
        {
            ValidateCustomer(customer);

            Validate(
                (Rule: IsInvalid(customer.Id), Parameter: nameof(Customer.Id)),
                (Rule: IsInvalid(customer.LastName), Parameter: nameof(Customer.LastName)),
                (Rule: IsInvalid(customer.FirstName), Parameter: nameof(Customer.FirstName)),
                (Rule: IsInvalid(customer.MiddleName), Parameter: nameof(Customer.MiddleName)),
                (Rule: IsInvalid(customer.Address), Parameter: nameof(Customer.Address)),
                (Rule: IsInvalid(customer.BirthDate), Parameter: nameof(Customer.BirthDate)),
                (Rule: IsInvalid(customer.UserId), Parameter: nameof(Customer.UserId)),
                (Rule: IsInvalid(customer.CreatedBy), Parameter: nameof(Customer.CreatedBy)),
                (Rule: IsInvalid(customer.UpdatedBy), Parameter: nameof(Customer.UpdatedBy)),
                (Rule: IsInvalid(customer.CreatedDate), Parameter: nameof(Customer.CreatedDate)),
                (Rule: IsInvalid(customer.UpdatedDate), Parameter: nameof(Customer.UpdatedDate)),
                (Rule: IsNotRecent(customer.UpdatedDate), Parameter: nameof(Customer.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: customer.UpdatedDate,
                    secondDate: customer.CreatedDate,
                    secondDateName: nameof(Customer.CreatedDate)),
                    Parameter: nameof(Customer.UpdatedDate))
            );
        }

        public void ValidateAgainstStorageCustomerOnModify(Customer inputCustomer, Customer storageCustomer)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputCustomer.CreatedDate,
                    secondDate: storageCustomer.CreatedDate,
                    secondDateName: nameof(Customer.CreatedDate)),
                    Parameter: nameof(Customer.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputCustomer.UpdatedDate,
                    secondDate: storageCustomer.UpdatedDate,
                    secondDateName: nameof(Customer.UpdatedDate)),
                    Parameter: nameof(Customer.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: inputCustomer.CreatedBy,
                    secondId: storageCustomer.CreatedBy,
                    secondIdName: nameof(Customer.CreatedBy)),
                    Parameter: nameof(Customer.CreatedBy))
            );
        }

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTime();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

        private static void ValidateCustomer(Customer customer)
        {
            if (customer is null)
            {
                throw new NullCustomerException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidCustomerException = new InvalidCustomerException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidCustomerException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidCustomerException.ThrowIfContainsErrors();
        }
    }
}