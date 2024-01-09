// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.VacationPackages;
using Jaunts.Core.Api.Models.Services.Foundations.VacationPackages.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace Jaunts.Core.Api.Services.Foundations.VacationPackages
{
    public partial class VacationPackageService
    {
        private delegate ValueTask<VacationPackage> ReturningVacationPackageFunction();
        private delegate IQueryable<VacationPackage> ReturningVacationPackagesFunction();

        private async ValueTask<VacationPackage> TryCatch(ReturningVacationPackageFunction returningVacationPackageFunction)
        {
            try
            {
                return await returningVacationPackageFunction();
            }
            catch (NullVacationPackageException nullVacationPackageException)
            {
                throw CreateAndLogValidationException(nullVacationPackageException);
            }
            catch (InvalidVacationPackageException invalidVacationPackageException)
            {
                throw CreateAndLogValidationException(invalidVacationPackageException);
            }
            catch (NotFoundVacationPackageException nullVacationPackageException)
            {
                throw CreateAndLogValidationException(nullVacationPackageException);
            }
            catch (SqlException sqlException)
            {
                var failedVacationPackageStorageException =
                    new FailedVacationPackageStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedVacationPackageStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsVacationPackageException =
                    new AlreadyExistsVacationPackageException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsVacationPackageException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedVacationPackageException = new LockedVacationPackageException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedVacationPackageException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedVacationPackageStorageException =
                    new FailedVacationPackageStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedVacationPackageStorageException);
            }
            catch (Exception exception)
            {
                var failedVacationPackageServiceException =
                    new FailedVacationPackageServiceException(exception);

                throw CreateAndLogServiceException(failedVacationPackageServiceException);
            }
        }

        private IQueryable<VacationPackage> TryCatch(ReturningVacationPackagesFunction returningVacationPackagesFunction)
        {
            try
            {
                return returningVacationPackagesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedVacationPackageStorageException =
                     new FailedVacationPackageStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedVacationPackageStorageException);
            }
            catch (Exception exception)
            {
                var failedVacationPackageServiceException =
                    new FailedVacationPackageServiceException(exception);

                throw CreateAndLogServiceException(failedVacationPackageServiceException);
            }
        }

        private VacationPackageValidationException CreateAndLogValidationException(Xeption exception)
        {
            var studentValidationException = new VacationPackageValidationException(exception);
            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }
        private VacationPackageDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)

        {
            var studentDependencyValidationException =
                new VacationPackageDependencyValidationException(exception);

            this.loggingBroker.LogError(studentDependencyValidationException);

            return studentDependencyValidationException;
        }

        private VacationPackageDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var studentDependencyException = new VacationPackageDependencyException(exception);
            this.loggingBroker.LogError(studentDependencyException);

            return studentDependencyException;
        }

        private VacationPackageDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var studentDependencyException = new VacationPackageDependencyException(exception);
            this.loggingBroker.LogCritical(studentDependencyException);

            return studentDependencyException;
        }

        private VacationPackageServiceException CreateAndLogServiceException(Xeption exception)
        {
            var studentServiceException = new VacationPackageServiceException(exception);
            this.loggingBroker.LogError(studentServiceException);

            return studentServiceException;
        }
    }
}
