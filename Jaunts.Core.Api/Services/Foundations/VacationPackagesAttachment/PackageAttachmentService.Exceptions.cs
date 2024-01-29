// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Jaunts.Core.Api.Services.Foundations.PackagesAttachments
{
    public partial class PackageAttachmentService
    {
        private delegate ValueTask<PackageAttachment> ReturningPackageAttachmentFunction();
        private delegate IQueryable<PackageAttachment> ReturningPackageAttachmentsFunction();

        private async ValueTask<PackageAttachment> TryCatch(
            ReturningPackageAttachmentFunction returningPackageAttachmentFunction)
        {
            try
            {
                return await returningPackageAttachmentFunction();
            }
            catch (NullPackageAttachmentException nullPackageAttachmentException)
            {
                throw CreateAndLogValidationException(nullPackageAttachmentException);
            }
            catch (InvalidPackageAttachmentException invalidPackageAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidPackageAttachmentInputException);
            }
            catch (NotFoundPackageAttachmentException notFoundPackageAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundPackageAttachmentException);
            }
            catch (SqlException sqlException)
            {
                var failedPackageAttachmentStorageException =
                   new FailedPackageAttachmentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedPackageAttachmentStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsPackageAttachmentException =
                    new AlreadyExistsPackageAttachmentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsPackageAttachmentException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidPackageAttachmentReferenceException =
                    new InvalidPackageAttachmentReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogValidationException(invalidPackageAttachmentReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedPackageAttachmentException =
                    new LockedPackageAttachmentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedPackageAttachmentException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedPackageAttachmentStorageException =
                   new FailedPackageAttachmentStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedPackageAttachmentStorageException);
            }
            catch (Exception exception)
            {
                var failedPackageAttachmentServiceException =
                    new FailedPackageAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedPackageAttachmentServiceException);
            }
        }

        private IQueryable<PackageAttachment> TryCatch(ReturningPackageAttachmentsFunction returningPackageAttachmentsFunction)
        {
            try
            {
                return returningPackageAttachmentsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedPackageAttachmentStorageException =
                   new FailedPackageAttachmentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedPackageAttachmentStorageException);
            }
            catch (Exception exception)
            {
                var failedPackageAttachmentServiceException =
                    new FailedPackageAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedPackageAttachmentServiceException);
            }
        }

        private PackageAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var packageAttachmentValidationException = new PackageAttachmentValidationException(exception);
            this.loggingBroker.LogError(packageAttachmentValidationException);

            return packageAttachmentValidationException;
        }

        private PackageAttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var packageAttachmentDependencyException = new PackageAttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(packageAttachmentDependencyException);

            return packageAttachmentDependencyException;
        }

        private PackageAttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var packageAttachmentDependencyException = new PackageAttachmentDependencyException(exception);
            this.loggingBroker.LogError(packageAttachmentDependencyException);

            return packageAttachmentDependencyException;
        }

        private PackageAttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var packageAttachmentServiceException = new PackageAttachmentServiceException(exception);
            this.loggingBroker.LogError(packageAttachmentServiceException);

            return packageAttachmentServiceException;
        }
    }
}
