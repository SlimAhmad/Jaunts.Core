// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Jaunts.Core.Api.Services.Foundations.FlightDealAttachments
{
    public partial class FlightDealAttachmentService
    {
        private delegate ValueTask<FlightDealAttachment> ReturningFlightDealAttachmentFunction();
        private delegate IQueryable<FlightDealAttachment> ReturningFlightDealAttachmentsFunction();

        private async ValueTask<FlightDealAttachment> TryCatch(
            ReturningFlightDealAttachmentFunction returningFlightDealAttachmentFunction)
        {
            try
            {
                return await returningFlightDealAttachmentFunction();
            }
            catch (NullFlightDealAttachmentException nullFlightDealAttachmentException)
            {
                throw CreateAndLogValidationException(nullFlightDealAttachmentException);
            }
            catch (InvalidFlightDealAttachmentException invalidFlightDealAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidFlightDealAttachmentInputException);
            }
            catch (NotFoundFlightDealAttachmentException notFoundFlightDealAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundFlightDealAttachmentException);
            }
            catch (SqlException sqlException)
            {
                var failedAttachmentStorageException =
                   new FailedFlightDealAttachmentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedAttachmentStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsFlightDealAttachmentException =
                    new AlreadyExistsFlightDealAttachmentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsFlightDealAttachmentException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidFlightDealAttachmentReferenceException =
                    new InvalidFlightDealAttachmentReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogValidationException(invalidFlightDealAttachmentReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedFlightDealAttachmentException =
                    new LockedFlightDealAttachmentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedFlightDealAttachmentException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedFlightDealAttachmentStorageException =
                   new FailedFlightDealAttachmentStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedFlightDealAttachmentStorageException);
            }
            catch (Exception exception)
            {
                var failedFlightDealAttachmentServiceException =
                    new FailedFlightDealAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedFlightDealAttachmentServiceException);
            }
        }

        private IQueryable<FlightDealAttachment> TryCatch(ReturningFlightDealAttachmentsFunction returningFlightDealAttachmentsFunction)
        {
            try
            {
                return returningFlightDealAttachmentsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedAttachmentStorageException =
                  new FailedFlightDealAttachmentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedAttachmentStorageException);
            }
            catch (Exception exception)
            {
                var failedFlightDealAttachmentServiceException =
                    new FailedFlightDealAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedFlightDealAttachmentServiceException);
            }
        }

        private FlightDealAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var guardianAttachmentValidationException = new FlightDealAttachmentValidationException(exception);
            this.loggingBroker.LogError(guardianAttachmentValidationException);

            return guardianAttachmentValidationException;
        }

        private FlightDealAttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var guardianAttachmentDependencyException = new FlightDealAttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(guardianAttachmentDependencyException);

            return guardianAttachmentDependencyException;
        }

        private FlightDealAttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var guardianAttachmentDependencyException = new FlightDealAttachmentDependencyException(exception);
            this.loggingBroker.LogError(guardianAttachmentDependencyException);

            return guardianAttachmentDependencyException;
        }

        private FlightDealAttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var guardianAttachmentServiceException = new FlightDealAttachmentServiceException(exception);
            this.loggingBroker.LogError(guardianAttachmentServiceException);

            return guardianAttachmentServiceException;
        }
    }
}
