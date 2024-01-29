// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLets.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Jaunts.Core.Api.Services.Foundations.ShortLetAttachments
{
    public partial class ShortLetAttachmentService
    {
        private delegate ValueTask<ShortLetAttachment> ReturningShortLetAttachmentFunction();
        private delegate IQueryable<ShortLetAttachment> ReturningShortLetAttachmentsFunction();

        private async ValueTask<ShortLetAttachment> TryCatch(
            ReturningShortLetAttachmentFunction returningShortLetAttachmentFunction)
        {
            try
            {
                return await returningShortLetAttachmentFunction();
            }
            catch (NullShortLetAttachmentException nullShortLetAttachmentException)
            {
                throw CreateAndLogValidationException(nullShortLetAttachmentException);
            }
            catch (InvalidShortLetAttachmentException invalidShortLetAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidShortLetAttachmentInputException);
            }
            catch (NotFoundShortLetAttachmentException notFoundShortLetAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundShortLetAttachmentException);
            }
            catch (SqlException sqlException)
            {
                var failedShortLetAttachmentStorageException =
                   new FailedShortLetAttachmentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedShortLetAttachmentStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsShortLetAttachmentException =
                    new AlreadyExistsShortLetAttachmentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsShortLetAttachmentException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidShortLetAttachmentReferenceException =
                    new InvalidShortLetAttachmentReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogValidationException(invalidShortLetAttachmentReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedShortLetAttachmentException =
                    new LockedShortLetAttachmentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedShortLetAttachmentException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedShortLetAttachmentStorageException =
                   new FailedShortLetAttachmentStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedShortLetAttachmentStorageException);
            }
            catch (Exception exception)
            {
                var failedShortLetAttachmentServiceException =
                    new FailedShortLetAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedShortLetAttachmentServiceException);
            }
        }

        private IQueryable<ShortLetAttachment> TryCatch(ReturningShortLetAttachmentsFunction returningShortLetAttachmentsFunction)
        {
            try
            {
                return returningShortLetAttachmentsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedShortLetAttachmentStorageException =
                   new FailedShortLetAttachmentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedShortLetAttachmentStorageException);
            }
            catch (Exception exception)
            {
                var failedShortLetAttachmentServiceException =
                    new FailedShortLetAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedShortLetAttachmentServiceException);
            }
        }

        private ShortLetAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var shortLetAttachmentValidationException = new ShortLetAttachmentValidationException(exception);
            this.loggingBroker.LogError(shortLetAttachmentValidationException);

            return shortLetAttachmentValidationException;
        }

        private ShortLetAttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var shortLetAttachmentDependencyException = new ShortLetAttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(shortLetAttachmentDependencyException);

            return shortLetAttachmentDependencyException;
        }

        private ShortLetAttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var shortLetAttachmentDependencyException = new ShortLetAttachmentDependencyException(exception);
            this.loggingBroker.LogError(shortLetAttachmentDependencyException);

            return shortLetAttachmentDependencyException;
        }

        private ShortLetAttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var shortLetAttachmentServiceException = new ShortLetAttachmentServiceException(exception);
            this.loggingBroker.LogError(shortLetAttachmentServiceException);

            return shortLetAttachmentServiceException;
        }
    }
}
