// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.RideAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.RideAttachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Jaunts.Core.Api.Services.Foundations.RideAttachments
{
    public partial class RideAttachmentService
    {
        private delegate ValueTask<RideAttachment> ReturningRideAttachmentFunction();
        private delegate IQueryable<RideAttachment> ReturningRideAttachmentsFunction();

        private async ValueTask<RideAttachment> TryCatch(
            ReturningRideAttachmentFunction returningRideAttachmentFunction)
        {
            try
            {
                return await returningRideAttachmentFunction();
            }
            catch (NullRideAttachmentException nullRideAttachmentException)
            {
                throw CreateAndLogValidationException(nullRideAttachmentException);
            }
            catch (InvalidRideAttachmentException invalidRideAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidRideAttachmentInputException);
            }
            catch (NotFoundRideAttachmentException notFoundRideAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundRideAttachmentException);
            }
            catch (SqlException sqlException)
            {
                var failedRideAttachmentStorageException =
                   new FailedRideAttachmentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedRideAttachmentStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsRideAttachmentException =
                    new AlreadyExistsRideAttachmentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsRideAttachmentException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidRideAttachmentReferenceException =
                    new InvalidRideAttachmentReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogValidationException(invalidRideAttachmentReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedRideAttachmentException =
                    new LockedRideAttachmentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedRideAttachmentException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedRideAttachmentStorageException =
                   new FailedRideAttachmentStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedRideAttachmentStorageException);
            }
            catch (Exception exception)
            {
                var failedRideAttachmentServiceException =
                    new FailedRideAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedRideAttachmentServiceException);
            }
        }

        private IQueryable<RideAttachment> TryCatch(ReturningRideAttachmentsFunction returningRideAttachmentsFunction)
        {
            try
            {
                return returningRideAttachmentsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedRideAttachmentStorageException =
                   new FailedRideAttachmentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedRideAttachmentStorageException);
            }
            catch (Exception exception)
            {
                var failedRideAttachmentServiceException =
                    new FailedRideAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedRideAttachmentServiceException);
            }
        }

        private RideAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var providersDirectorAttachmentValidationException = new RideAttachmentValidationException(exception);
            this.loggingBroker.LogError(providersDirectorAttachmentValidationException);

            return providersDirectorAttachmentValidationException;
        }

        private RideAttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var providersDirectorAttachmentDependencyException = new RideAttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(providersDirectorAttachmentDependencyException);

            return providersDirectorAttachmentDependencyException;
        }

        private RideAttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var providersDirectorAttachmentDependencyException = new RideAttachmentDependencyException(exception);
            this.loggingBroker.LogError(providersDirectorAttachmentDependencyException);

            return providersDirectorAttachmentDependencyException;
        }

        private RideAttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var providersDirectorAttachmentServiceException = new RideAttachmentServiceException(exception);
            this.loggingBroker.LogError(providersDirectorAttachmentServiceException);

            return providersDirectorAttachmentServiceException;
        }
    }
}
