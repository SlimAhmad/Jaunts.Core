// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Jaunts.Core.Api.Services.Foundations.CustomerAttachments
{
    public partial class CustomerAttachmentService
    {
        private delegate ValueTask<CustomerAttachment> ReturningCustomerAttachmentFunction();
        private delegate IQueryable<CustomerAttachment> ReturningCustomerAttachmentsFunction();

        private async ValueTask<CustomerAttachment> TryCatch(
            ReturningCustomerAttachmentFunction returningCustomerAttachmentFunction)
        {
            try
            {
                return await returningCustomerAttachmentFunction();
            }
            catch (NullCustomerAttachmentException nullCustomerAttachmentException)
            {
                throw CreateAndLogValidationException(nullCustomerAttachmentException);
            }
            catch (InvalidCustomerAttachmentException invalidCustomerAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidCustomerAttachmentInputException);
            }
            catch (NotFoundCustomerAttachmentException notFoundCustomerAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundCustomerAttachmentException);
            }
            catch (SqlException sqlException)
            {
                var failedCustomerAttachmentStorageException =
                   new FailedCustomerAttachmentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedCustomerAttachmentStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsCustomerAttachmentException =
                    new AlreadyExistsCustomerAttachmentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsCustomerAttachmentException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidCustomerAttachmentReferenceException =
                    new InvalidCustomerAttachmentReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogValidationException(invalidCustomerAttachmentReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedCustomerAttachmentException =
                    new LockedCustomerAttachmentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedCustomerAttachmentException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedCustomerAttachmentStorageException =
                   new FailedCustomerAttachmentStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedCustomerAttachmentStorageException);
            }
            catch (Exception exception)
            {
                var failedCustomerAttachmentServiceException =
                    new FailedCustomerAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedCustomerAttachmentServiceException);
            }
        }

        private IQueryable<CustomerAttachment> TryCatch(ReturningCustomerAttachmentsFunction returningCustomerAttachmentsFunction)
        {
            try
            {
                return returningCustomerAttachmentsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedCustomerAttachmentStorageException =
                   new FailedCustomerAttachmentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedCustomerAttachmentStorageException);
            }
            catch (Exception exception)
            {
                var failedCustomerAttachmentServiceException =
                    new FailedCustomerAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedCustomerAttachmentServiceException);
            }
        }

        private CustomerAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var customerAttachmentValidationException = new CustomerAttachmentValidationException(exception);
            this.loggingBroker.LogError(customerAttachmentValidationException);

            return customerAttachmentValidationException;
        }

        private CustomerAttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var customerAttachmentDependencyException = new CustomerAttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(customerAttachmentDependencyException);

            return customerAttachmentDependencyException;
        }

        private CustomerAttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var customerAttachmentDependencyException = new CustomerAttachmentDependencyException(exception);
            this.loggingBroker.LogError(customerAttachmentDependencyException);

            return customerAttachmentDependencyException;
        }

        private CustomerAttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var customerAttachmentServiceException = new CustomerAttachmentServiceException(exception);
            this.loggingBroker.LogError(customerAttachmentServiceException);

            return customerAttachmentServiceException;
        }
    }
}
