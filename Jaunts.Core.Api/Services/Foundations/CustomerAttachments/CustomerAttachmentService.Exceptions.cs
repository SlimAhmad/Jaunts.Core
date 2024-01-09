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
                throw CreateAndLogCriticalDependencyException(sqlException);
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
                throw CreateAndLogDependencyException(dbUpdateException);
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
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

        private CustomerAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var guardianAttachmentValidationException = new CustomerAttachmentValidationException(exception);
            this.loggingBroker.LogError(guardianAttachmentValidationException);

            return guardianAttachmentValidationException;
        }

        private CustomerAttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var guardianAttachmentDependencyException = new CustomerAttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(guardianAttachmentDependencyException);

            return guardianAttachmentDependencyException;
        }

        private CustomerAttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var guardianAttachmentDependencyException = new CustomerAttachmentDependencyException(exception);
            this.loggingBroker.LogError(guardianAttachmentDependencyException);

            return guardianAttachmentDependencyException;
        }

        private CustomerAttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var guardianAttachmentServiceException = new CustomerAttachmentServiceException(exception);
            this.loggingBroker.LogError(guardianAttachmentServiceException);

            return guardianAttachmentServiceException;
        }
    }
}
