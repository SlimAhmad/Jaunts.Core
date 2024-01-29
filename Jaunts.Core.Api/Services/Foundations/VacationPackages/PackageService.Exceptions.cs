// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Packages;
using Jaunts.Core.Api.Models.Services.Foundations.Packages.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace Jaunts.Core.Api.Services.Foundations.Packages
{
    public partial class PackageService
    {
        private delegate ValueTask<Package> ReturningPackageFunction();
        private delegate IQueryable<Package> ReturningPackagesFunction();

        private async ValueTask<Package> TryCatch(ReturningPackageFunction returningPackageFunction)
        {
            try
            {
                return await returningPackageFunction();
            }
            catch (NullPackageException nullPackageException)
            {
                throw CreateAndLogValidationException(nullPackageException);
            }
            catch (InvalidPackageException invalidPackageException)
            {
                throw CreateAndLogValidationException(invalidPackageException);
            }
            catch (NotFoundPackageException nullPackageException)
            {
                throw CreateAndLogValidationException(nullPackageException);
            }
            catch (SqlException sqlException)
            {
                var FailedPackageStorageException =
                    new FailedPackageStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(FailedPackageStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsPackageException =
                    new AlreadyExistsPackageException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsPackageException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedPackageException = new LockedPackageException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedPackageException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var FailedPackageStorageException =
                    new FailedPackageStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(FailedPackageStorageException);
            }
            catch (Exception exception)
            {
                var failedPackageServiceException =
                    new FailedPackageServiceException(exception);

                throw CreateAndLogServiceException(failedPackageServiceException);
            }
        }

        private IQueryable<Package> TryCatch(ReturningPackagesFunction returningPackagesFunction)
        {
            try
            {
                return returningPackagesFunction();
            }
            catch (SqlException sqlException)
            {
                var FailedPackageStorageException =
                     new FailedPackageStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(FailedPackageStorageException);
            }
            catch (Exception exception)
            {
                var failedPackageServiceException =
                    new FailedPackageServiceException(exception);

                throw CreateAndLogServiceException(failedPackageServiceException);
            }
        }

        private PackageValidationException CreateAndLogValidationException(Xeption exception)
        {
            var studentValidationException = new PackageValidationException(exception);
            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }
        private PackageDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)

        {
            var studentDependencyValidationException =
                new PackageDependencyValidationException(exception);

            this.loggingBroker.LogError(studentDependencyValidationException);

            return studentDependencyValidationException;
        }

        private PackageDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var studentDependencyException = new PackageDependencyException(exception);
            this.loggingBroker.LogError(studentDependencyException);

            return studentDependencyException;
        }

        private PackageDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var studentDependencyException = new PackageDependencyException(exception);
            this.loggingBroker.LogCritical(studentDependencyException);

            return studentDependencyException;
        }

        private PackageServiceException CreateAndLogServiceException(Xeption exception)
        {
            var studentServiceException = new PackageServiceException(exception);
            this.loggingBroker.LogError(studentServiceException);

            return studentServiceException;
        }
    }
}
