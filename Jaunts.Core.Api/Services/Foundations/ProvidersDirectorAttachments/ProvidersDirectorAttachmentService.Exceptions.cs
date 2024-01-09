// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Jaunts.Core.Api.Services.Foundations.ProvidersDirectorAttachments
{
    public partial class ProvidersDirectorAttachmentService
    {
        private delegate ValueTask<ProvidersDirectorAttachment> ReturningProvidersDirectorAttachmentFunction();
        private delegate IQueryable<ProvidersDirectorAttachment> ReturningProvidersDirectorAttachmentsFunction();

        private async ValueTask<ProvidersDirectorAttachment> TryCatch(
            ReturningProvidersDirectorAttachmentFunction returningProvidersDirectorAttachmentFunction)
        {
            try
            {
                return await returningProvidersDirectorAttachmentFunction();
            }
            catch (NullProvidersDirectorAttachmentException nullProvidersDirectorAttachmentException)
            {
                throw CreateAndLogValidationException(nullProvidersDirectorAttachmentException);
            }
            catch (InvalidProvidersDirectorAttachmentException invalidProvidersDirectorAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidProvidersDirectorAttachmentInputException);
            }
            catch (NotFoundProvidersDirectorAttachmentException notFoundProvidersDirectorAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundProvidersDirectorAttachmentException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsProvidersDirectorAttachmentException =
                    new AlreadyExistsProvidersDirectorAttachmentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsProvidersDirectorAttachmentException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidProvidersDirectorAttachmentReferenceException =
                    new InvalidProvidersDirectorAttachmentReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogValidationException(invalidProvidersDirectorAttachmentReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedProvidersDirectorAttachmentException =
                    new LockedProvidersDirectorAttachmentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedProvidersDirectorAttachmentException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
            catch (Exception exception)
            {
                var failedProvidersDirectorAttachmentServiceException =
                    new FailedProvidersDirectorAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedProvidersDirectorAttachmentServiceException);
            }
        }

        private IQueryable<ProvidersDirectorAttachment> TryCatch(ReturningProvidersDirectorAttachmentsFunction returningProvidersDirectorAttachmentsFunction)
        {
            try
            {
                return returningProvidersDirectorAttachmentsFunction();
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

        private ProvidersDirectorAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var guardianAttachmentValidationException = new ProvidersDirectorAttachmentValidationException(exception);
            this.loggingBroker.LogError(guardianAttachmentValidationException);

            return guardianAttachmentValidationException;
        }

        private ProvidersDirectorAttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var guardianAttachmentDependencyException = new ProvidersDirectorAttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(guardianAttachmentDependencyException);

            return guardianAttachmentDependencyException;
        }

        private ProvidersDirectorAttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var guardianAttachmentDependencyException = new ProvidersDirectorAttachmentDependencyException(exception);
            this.loggingBroker.LogError(guardianAttachmentDependencyException);

            return guardianAttachmentDependencyException;
        }

        private ProvidersDirectorAttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var guardianAttachmentServiceException = new ProvidersDirectorAttachmentServiceException(exception);
            this.loggingBroker.LogError(guardianAttachmentServiceException);

            return guardianAttachmentServiceException;
        }
    }
}
