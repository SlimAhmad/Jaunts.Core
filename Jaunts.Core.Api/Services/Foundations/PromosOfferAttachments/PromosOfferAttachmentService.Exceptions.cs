// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Jaunts.Core.Api.Services.Foundations.PromosOfferAttachments
{
    public partial class PromosOfferAttachmentService
    {
        private delegate ValueTask<PromosOfferAttachment> ReturningPromosOfferAttachmentFunction();
        private delegate IQueryable<PromosOfferAttachment> ReturningPromosOfferAttachmentsFunction();

        private async ValueTask<PromosOfferAttachment> TryCatch(
            ReturningPromosOfferAttachmentFunction returningPromosOfferAttachmentFunction)
        {
            try
            {
                return await returningPromosOfferAttachmentFunction();
            }
            catch (NullPromosOfferAttachmentException nullPromosOfferAttachmentException)
            {
                throw CreateAndLogValidationException(nullPromosOfferAttachmentException);
            }
            catch (InvalidPromosOfferAttachmentException invalidPromosOfferAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidPromosOfferAttachmentInputException);
            }
            catch (NotFoundPromosOfferAttachmentException notFoundPromosOfferAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundPromosOfferAttachmentException);
            }
            catch (SqlException sqlException)
            {
                var failedPromosOfferAttachmentStorageException =
                   new FailedPromosOfferAttachmentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedPromosOfferAttachmentStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsPromosOfferAttachmentException =
                    new AlreadyExistsPromosOfferAttachmentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsPromosOfferAttachmentException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidPromosOfferAttachmentReferenceException =
                    new InvalidPromosOfferAttachmentReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogValidationException(invalidPromosOfferAttachmentReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedPromosOfferAttachmentException =
                    new LockedPromosOfferAttachmentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedPromosOfferAttachmentException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedPromosOfferAttachmentStorageException =
                   new FailedPromosOfferAttachmentStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedPromosOfferAttachmentStorageException);
            }
            catch (Exception exception)
            {
                var failedPromosOfferAttachmentServiceException =
                    new FailedPromosOfferAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedPromosOfferAttachmentServiceException);
            }
        }

        private IQueryable<PromosOfferAttachment> TryCatch(ReturningPromosOfferAttachmentsFunction returningPromosOfferAttachmentsFunction)
        {
            try
            {
                return returningPromosOfferAttachmentsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedPromosOfferAttachmentStorageException =
                   new FailedPromosOfferAttachmentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedPromosOfferAttachmentStorageException);
            }
            catch (Exception exception)
            {
                var failedPromosOfferAttachmentServiceException =
                    new FailedPromosOfferAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedPromosOfferAttachmentServiceException);
            }
        }

        private PromosOfferAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var promosOfferAttachmentValidationException = new PromosOfferAttachmentValidationException(exception);
            this.loggingBroker.LogError(promosOfferAttachmentValidationException);

            return promosOfferAttachmentValidationException;
        }

        private PromosOfferAttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var promosOfferAttachmentDependencyException = new PromosOfferAttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(promosOfferAttachmentDependencyException);

            return promosOfferAttachmentDependencyException;
        }

        private PromosOfferAttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var promosOfferAttachmentDependencyException = new PromosOfferAttachmentDependencyException(exception);
            this.loggingBroker.LogError(promosOfferAttachmentDependencyException);

            return promosOfferAttachmentDependencyException;
        }

        private PromosOfferAttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var promosOfferAttachmentServiceException = new PromosOfferAttachmentServiceException(exception);
            this.loggingBroker.LogError(promosOfferAttachmentServiceException);

            return promosOfferAttachmentServiceException;
        }
    }
}
