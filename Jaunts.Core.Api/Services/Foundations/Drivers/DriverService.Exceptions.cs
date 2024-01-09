// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Drivers;
using Jaunts.Core.Api.Models.Services.Foundations.Drivers.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace Jaunts.Core.Api.Services.Foundations.Drivers
{
    public partial class DriverService
    {
        private delegate ValueTask<Driver> ReturningDriverFunction();
        private delegate IQueryable<Driver> ReturningDriversFunction();

        private async ValueTask<Driver> TryCatch(ReturningDriverFunction returningDriverFunction)
        {
            try
            {
                return await returningDriverFunction();
            }
            catch (NullDriverException nullDriverException)
            {
                throw CreateAndLogValidationException(nullDriverException);
            }
            catch (InvalidDriverException invalidDriverException)
            {
                throw CreateAndLogValidationException(invalidDriverException);
            }
            catch (NotFoundDriverException nullDriverException)
            {
                throw CreateAndLogValidationException(nullDriverException);
            }
            catch (SqlException sqlException)
            {
                var failedDriverStorageException =
                    new FailedDriverStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedDriverStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsDriverException =
                    new AlreadyExistsDriverException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsDriverException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedDriverException = new LockedDriverException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedDriverException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedDriverStorageException =
                    new FailedDriverStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedDriverStorageException);
            }
            catch (Exception exception)
            {
                var failedDriverServiceException =
                    new FailedDriverServiceException(exception);

                throw CreateAndLogServiceException(failedDriverServiceException);
            }
        }

        private IQueryable<Driver> TryCatch(ReturningDriversFunction returningDriversFunction)
        {
            try
            {
                return returningDriversFunction();
            }
            catch (SqlException sqlException)
            {
                var failedDriverStorageException =
                     new FailedDriverStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedDriverStorageException);
            }
            catch (Exception exception)
            {
                var failedDriverServiceException =
                    new FailedDriverServiceException(exception);

                throw CreateAndLogServiceException(failedDriverServiceException);
            }
        }

        private DriverValidationException CreateAndLogValidationException(Xeption exception)
        {
            var studentValidationException = new DriverValidationException(exception);
            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }
        private DriverDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)

        {
            var studentDependencyValidationException =
                new DriverDependencyValidationException(exception);

            this.loggingBroker.LogError(studentDependencyValidationException);

            return studentDependencyValidationException;
        }

        private DriverDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var studentDependencyException = new DriverDependencyException(exception);
            this.loggingBroker.LogError(studentDependencyException);

            return studentDependencyException;
        }

        private DriverDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var studentDependencyException = new DriverDependencyException(exception);
            this.loggingBroker.LogCritical(studentDependencyException);

            return studentDependencyException;
        }

        private DriverServiceException CreateAndLogServiceException(Xeption exception)
        {
            var studentServiceException = new DriverServiceException(exception);
            this.loggingBroker.LogError(studentServiceException);

            return studentServiceException;
        }
    }
}
