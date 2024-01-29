// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Jaunts.Core.Api.Services.Foundations.ProviderAttachments
{
    public partial class ProviderAttachmentService
    {
        private delegate ValueTask<ProviderAttachment> ReturningProviderAttachmentFunction();
        private delegate IQueryable<ProviderAttachment> ReturningProviderAttachmentsFunction();

        private async ValueTask<ProviderAttachment> TryCatch(
            ReturningProviderAttachmentFunction returningProviderAttachmentFunction)
        {
            try
            {
                return await returningProviderAttachmentFunction();
            }
            catch (NullProviderAttachmentException nullProviderAttachmentException)
            {
                throw CreateAndLogValidationException(nullProviderAttachmentException);
            }
            catch (InvalidProviderAttachmentException invalidProviderAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidProviderAttachmentInputException);
            }
            catch (NotFoundProviderAttachmentException notFoundProviderAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundProviderAttachmentException);
            }
            catch (SqlException sqlException)
            {
                var failedAttachmentStorageException =
                   new FailedProviderAttachmentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedAttachmentStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsProviderAttachmentException =
                    new AlreadyExistsProviderAttachmentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsProviderAttachmentException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidProviderAttachmentReferenceException =
                    new InvalidProviderAttachmentReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogValidationException(invalidProviderAttachmentReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedProviderAttachmentException =
                    new LockedProviderAttachmentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedProviderAttachmentException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedProviderAttachmentStorageException =
                   new FailedProviderAttachmentStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedProviderAttachmentStorageException);
            }
            catch (Exception exception)
            {
                var failedProviderAttachmentServiceException =
                    new FailedProviderAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedProviderAttachmentServiceException);
            }
        }

        private IQueryable<ProviderAttachment> TryCatch(ReturningProviderAttachmentsFunction returningProviderAttachmentsFunction)
        {
            try
            {
                return returningProviderAttachmentsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedAttachmentStorageException =
                  new FailedProviderAttachmentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedAttachmentStorageException);
            }
            catch (Exception exception)
            {
                var failedProviderAttachmentServiceException =
                    new FailedProviderAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedProviderAttachmentServiceException);
            }
        }

        private ProviderAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var guardianAttachmentValidationException = new ProviderAttachmentValidationException(exception);
            this.loggingBroker.LogError(guardianAttachmentValidationException);

            return guardianAttachmentValidationException;
        }

        private ProviderAttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var guardianAttachmentDependencyException = new ProviderAttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(guardianAttachmentDependencyException);

            return guardianAttachmentDependencyException;
        }

        private ProviderAttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var guardianAttachmentDependencyException = new ProviderAttachmentDependencyException(exception);
            this.loggingBroker.LogError(guardianAttachmentDependencyException);

            return guardianAttachmentDependencyException;
        }

        private ProviderAttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var guardianAttachmentServiceException = new ProviderAttachmentServiceException(exception);
            this.loggingBroker.LogError(guardianAttachmentServiceException);

            return guardianAttachmentServiceException;
        }
    }
}
