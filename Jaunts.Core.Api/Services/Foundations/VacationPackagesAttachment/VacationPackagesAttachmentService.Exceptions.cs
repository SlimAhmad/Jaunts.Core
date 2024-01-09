// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.VacationPackagesAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.VacationPackagesAttachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Jaunts.Core.Api.Services.Foundations.VacationPackagesAttachments
{
    public partial class VacationPackagesAttachmentService
    {
        private delegate ValueTask<VacationPackagesAttachment> ReturningVacationPackagesAttachmentFunction();
        private delegate IQueryable<VacationPackagesAttachment> ReturningVacationPackagesAttachmentsFunction();

        private async ValueTask<VacationPackagesAttachment> TryCatch(
            ReturningVacationPackagesAttachmentFunction returningVacationPackagesAttachmentFunction)
        {
            try
            {
                return await returningVacationPackagesAttachmentFunction();
            }
            catch (NullVacationPackagesAttachmentException nullVacationPackagesAttachmentException)
            {
                throw CreateAndLogValidationException(nullVacationPackagesAttachmentException);
            }
            catch (InvalidVacationPackagesAttachmentException invalidVacationPackagesAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidVacationPackagesAttachmentInputException);
            }
            catch (NotFoundVacationPackagesAttachmentException notFoundVacationPackagesAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundVacationPackagesAttachmentException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsVacationPackagesAttachmentException =
                    new AlreadyExistsVacationPackagesAttachmentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsVacationPackagesAttachmentException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidVacationPackagesAttachmentReferenceException =
                    new InvalidVacationPackagesAttachmentReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogValidationException(invalidVacationPackagesAttachmentReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedVacationPackagesAttachmentException =
                    new LockedVacationPackagesAttachmentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedVacationPackagesAttachmentException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
            catch (Exception exception)
            {
                var failedVacationPackagesAttachmentServiceException =
                    new FailedVacationPackagesAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedVacationPackagesAttachmentServiceException);
            }
        }

        private IQueryable<VacationPackagesAttachment> TryCatch(ReturningVacationPackagesAttachmentsFunction returningVacationPackagesAttachmentsFunction)
        {
            try
            {
                return returningVacationPackagesAttachmentsFunction();
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

        private VacationPackagesAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var guardianAttachmentValidationException = new VacationPackagesAttachmentValidationException(exception);
            this.loggingBroker.LogError(guardianAttachmentValidationException);

            return guardianAttachmentValidationException;
        }

        private VacationPackagesAttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var guardianAttachmentDependencyException = new VacationPackagesAttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(guardianAttachmentDependencyException);

            return guardianAttachmentDependencyException;
        }

        private VacationPackagesAttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var guardianAttachmentDependencyException = new VacationPackagesAttachmentDependencyException(exception);
            this.loggingBroker.LogError(guardianAttachmentDependencyException);

            return guardianAttachmentDependencyException;
        }

        private VacationPackagesAttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var guardianAttachmentServiceException = new VacationPackagesAttachmentServiceException(exception);
            this.loggingBroker.LogError(guardianAttachmentServiceException);

            return guardianAttachmentServiceException;
        }
    }
}
