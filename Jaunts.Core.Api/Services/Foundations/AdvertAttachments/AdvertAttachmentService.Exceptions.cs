// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Jaunts.Core.Api.Services.Foundations.AdvertAttachments
{
    public partial class AdvertAttachmentService
    {
        private delegate ValueTask<AdvertAttachment> ReturningAdvertAttachmentFunction();
        private delegate IQueryable<AdvertAttachment> ReturningAdvertAttachmentsFunction();

        private async ValueTask<AdvertAttachment> TryCatch(
            ReturningAdvertAttachmentFunction returningAdvertAttachmentFunction)
        {
            try
            {
                return await returningAdvertAttachmentFunction();
            }
            catch (NullAdvertAttachmentException nullAdvertAttachmentException)
            {
                throw CreateAndLogValidationException(nullAdvertAttachmentException);
            }
            catch (InvalidAdvertAttachmentException invalidAdvertAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidAdvertAttachmentInputException);
            }
            catch (NotFoundAdvertAttachmentException notFoundAdvertAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundAdvertAttachmentException);
            }
            catch (SqlException sqlException)
            {
                var failedAdvertAttachmentStorageException =
                   new FailedAdvertAttachmentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedAdvertAttachmentStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsAdvertAttachmentException =
                    new AlreadyExistsAdvertAttachmentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsAdvertAttachmentException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidAdvertAttachmentReferenceException =
                    new InvalidAdvertAttachmentReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogValidationException(invalidAdvertAttachmentReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedAdvertAttachmentException =
                    new LockedAdvertAttachmentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedAdvertAttachmentException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedAdvertAttachmentStorageException =
                   new FailedAdvertAttachmentStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedAdvertAttachmentStorageException);
            }
            catch (Exception exception)
            {
                var failedAdvertAttachmentServiceException =
                    new FailedAdvertAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedAdvertAttachmentServiceException);
            }
        }

        private IQueryable<AdvertAttachment> TryCatch(ReturningAdvertAttachmentsFunction returningAdvertAttachmentsFunction)
        {
            try
            {
                return returningAdvertAttachmentsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedAdvertAttachmentStorageException =
                   new FailedAdvertAttachmentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedAdvertAttachmentStorageException);
            }
            catch (Exception exception)
            {
                var failedAdvertAttachmentServiceException =
                    new FailedAdvertAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedAdvertAttachmentServiceException);
            }
        }

        private AdvertAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var advertAttachmentValidationException = new AdvertAttachmentValidationException(exception);
            this.loggingBroker.LogError(advertAttachmentValidationException);

            return advertAttachmentValidationException;
        }

        private AdvertAttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var advertAttachmentDependencyException = new AdvertAttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(advertAttachmentDependencyException);

            return advertAttachmentDependencyException;
        }

        private AdvertAttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var advertAttachmentDependencyException = new AdvertAttachmentDependencyException(exception);
            this.loggingBroker.LogError(advertAttachmentDependencyException);

            return advertAttachmentDependencyException;
        }

        private AdvertAttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var advertAttachmentServiceException = new AdvertAttachmentServiceException(exception);
            this.loggingBroker.LogError(advertAttachmentServiceException);

            return advertAttachmentServiceException;
        }
    }
}
