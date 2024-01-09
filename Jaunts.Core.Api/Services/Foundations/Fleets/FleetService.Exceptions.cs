// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Fleets;
using Jaunts.Core.Api.Models.Services.Foundations.Fleets.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace Jaunts.Core.Api.Services.Foundations.Fleets
{
    public partial class FleetService
    {
        private delegate ValueTask<Fleet> ReturningFleetFunction();
        private delegate IQueryable<Fleet> ReturningFleetsFunction();

        private async ValueTask<Fleet> TryCatch(ReturningFleetFunction returningFleetFunction)
        {
            try
            {
                return await returningFleetFunction();
            }
            catch (NullFleetException nullFleetException)
            {
                throw CreateAndLogValidationException(nullFleetException);
            }
            catch (InvalidFleetException invalidFleetException)
            {
                throw CreateAndLogValidationException(invalidFleetException);
            }
            catch (NotFoundFleetException nullFleetException)
            {
                throw CreateAndLogValidationException(nullFleetException);
            }
            catch (SqlException sqlException)
            {
                var failedFleetStorageException =
                    new FailedFleetStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedFleetStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsFleetException =
                    new AlreadyExistsFleetException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsFleetException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedFleetException = new LockedFleetException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedFleetException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedFleetStorageException =
                    new FailedFleetStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedFleetStorageException);
            }
            catch (Exception exception)
            {
                var failedFleetServiceException =
                    new FailedFleetServiceException(exception);

                throw CreateAndLogServiceException(failedFleetServiceException);
            }
        }

        private IQueryable<Fleet> TryCatch(ReturningFleetsFunction returningFleetsFunction)
        {
            try
            {
                return returningFleetsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedFleetStorageException =
                     new FailedFleetStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedFleetStorageException);
            }
            catch (Exception exception)
            {
                var failedFleetServiceException =
                    new FailedFleetServiceException(exception);

                throw CreateAndLogServiceException(failedFleetServiceException);
            }
        }

        private FleetValidationException CreateAndLogValidationException(Xeption exception)
        {
            var studentValidationException = new FleetValidationException(exception);
            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }
        private FleetDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)

        {
            var studentDependencyValidationException =
                new FleetDependencyValidationException(exception);

            this.loggingBroker.LogError(studentDependencyValidationException);

            return studentDependencyValidationException;
        }

        private FleetDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var studentDependencyException = new FleetDependencyException(exception);
            this.loggingBroker.LogError(studentDependencyException);

            return studentDependencyException;
        }

        private FleetDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var studentDependencyException = new FleetDependencyException(exception);
            this.loggingBroker.LogCritical(studentDependencyException);

            return studentDependencyException;
        }

        private FleetServiceException CreateAndLogServiceException(Xeption exception)
        {
            var studentServiceException = new FleetServiceException(exception);
            this.loggingBroker.LogError(studentServiceException);

            return studentServiceException;
        }
    }
}
