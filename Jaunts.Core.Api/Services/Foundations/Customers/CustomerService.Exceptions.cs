// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.customers.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Customers;
using Jaunts.Core.Api.Models.Services.Foundations.Customers.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace Jaunts.Core.Api.Services.Foundations.Customers
{
    public partial class CustomerService
    {
        private delegate ValueTask<Customer> ReturningCustomerFunction();
        private delegate IQueryable<Customer> ReturningCustomersFunction();

        private async ValueTask<Customer> TryCatch(ReturningCustomerFunction returningCustomerFunction)
        {
            try
            {
                return await returningCustomerFunction();
            }
            catch (NullCustomerException nullCustomerException)
            {
                throw CreateAndLogValidationException(nullCustomerException);
            }
            catch (InvalidCustomerException invalidCustomerException)
            {
                throw CreateAndLogValidationException(invalidCustomerException);
            }
            catch (NotFoundCustomerException nullCustomerException)
            {
                throw CreateAndLogValidationException(nullCustomerException);
            }
            catch (SqlException sqlException)
            {
                var failedCustomerStorageException =
                    new FailedCustomerStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedCustomerStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsCustomerException =
                    new AlreadyExistsCustomerException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsCustomerException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedCustomerException = new LockedCustomerException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedCustomerException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedCustomerStorageException =
                    new FailedCustomerStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedCustomerStorageException);
            }
            catch (Exception exception)
            {
                var failedCustomerServiceException =
                    new FailedCustomerServiceException(exception);

                throw CreateAndLogServiceException(failedCustomerServiceException);
            }
        }

        private IQueryable<Customer> TryCatch(ReturningCustomersFunction returningCustomersFunction)
        {
            try
            {
                return returningCustomersFunction();
            }
            catch (SqlException sqlException)
            {
                var failedCustomerStorageException =
                     new FailedCustomerStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedCustomerStorageException);
            }
            catch (Exception exception)
            {
                var failedCustomerServiceException =
                    new FailedCustomerServiceException(exception);

                throw CreateAndLogServiceException(failedCustomerServiceException);
            }
        }

        private CustomerValidationException CreateAndLogValidationException(Xeption exception)
        {
            var studentValidationException = new CustomerValidationException(exception);
            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }
        private CustomerDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)

        {
            var studentDependencyValidationException =
                new CustomerDependencyValidationException(exception);

            this.loggingBroker.LogError(studentDependencyValidationException);

            return studentDependencyValidationException;
        }

        private CustomerDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var studentDependencyException = new CustomerDependencyException(exception);
            this.loggingBroker.LogError(studentDependencyException);

            return studentDependencyException;
        }

        private CustomerDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var studentDependencyException = new CustomerDependencyException(exception);
            this.loggingBroker.LogCritical(studentDependencyException);

            return studentDependencyException;
        }

        private CustomerServiceException CreateAndLogServiceException(Xeption exception)
        {
            var studentServiceException = new CustomerServiceException(exception);
            this.loggingBroker.LogError(studentServiceException);

            return studentServiceException;
        }
    }
}
