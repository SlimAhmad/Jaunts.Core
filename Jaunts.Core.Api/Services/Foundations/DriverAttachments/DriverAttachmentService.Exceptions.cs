// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Jaunts.Core.Api.Services.Foundations.DriverAttachments
{
    public partial class DriverAttachmentService
    {
        private delegate ValueTask<DriverAttachment> ReturningDriverAttachmentFunction();
        private delegate IQueryable<DriverAttachment> ReturningDriverAttachmentsFunction();

        private async ValueTask<DriverAttachment> TryCatch(
            ReturningDriverAttachmentFunction returningDriverAttachmentFunction)
        {
            try
            {
                return await returningDriverAttachmentFunction();
            }
            catch (NullDriverAttachmentException nullDriverAttachmentException)
            {
                throw CreateAndLogValidationException(nullDriverAttachmentException);
            }
            catch (InvalidDriverAttachmentException invalidDriverAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidDriverAttachmentInputException);
            }
            catch (NotFoundDriverAttachmentException notFoundDriverAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundDriverAttachmentException);
            }
            catch (SqlException sqlException)
            {
                var failedAttachmentStorageException =
                   new FailedDriverAttachmentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedAttachmentStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsDriverAttachmentException =
                    new AlreadyExistsDriverAttachmentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsDriverAttachmentException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidDriverAttachmentReferenceException =
                    new InvalidDriverAttachmentReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogValidationException(invalidDriverAttachmentReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedDriverAttachmentException =
                    new LockedDriverAttachmentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedDriverAttachmentException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedDriverAttachmentStorageException =
                   new FailedDriverAttachmentStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedDriverAttachmentStorageException);
            }
            catch (Exception exception)
            {
                var failedDriverAttachmentServiceException =
                    new FailedDriverAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedDriverAttachmentServiceException);
            }
        }

        private IQueryable<DriverAttachment> TryCatch(ReturningDriverAttachmentsFunction returningDriverAttachmentsFunction)
        {
            try
            {
                return returningDriverAttachmentsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedAttachmentStorageException =
                  new FailedDriverAttachmentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedAttachmentStorageException);
            }
            catch (Exception exception)
            {
                var failedDriverAttachmentServiceException =
                    new FailedDriverAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedDriverAttachmentServiceException);
            }
        }

        private DriverAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var driverAttachmentValidationException = new DriverAttachmentValidationException(exception);
            this.loggingBroker.LogError(driverAttachmentValidationException);

            return driverAttachmentValidationException;
        }

        private DriverAttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var driverAttachmentDependencyException = new DriverAttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(driverAttachmentDependencyException);

            return driverAttachmentDependencyException;
        }

        private DriverAttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var driverAttachmentDependencyException = new DriverAttachmentDependencyException(exception);
            this.loggingBroker.LogError(driverAttachmentDependencyException);

            return driverAttachmentDependencyException;
        }

        private DriverAttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var driverAttachmentServiceException = new DriverAttachmentServiceException(exception);
            this.loggingBroker.LogError(driverAttachmentServiceException);

            return driverAttachmentServiceException;
        }
    }
}
