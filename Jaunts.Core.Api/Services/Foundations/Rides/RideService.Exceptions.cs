// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Rides;
using Jaunts.Core.Api.Models.Services.Foundations.Rides.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace Jaunts.Core.Api.Services.Foundations.Rides
{
    public partial class RideService
    {
        private delegate ValueTask<Ride> ReturningRideFunction();
        private delegate IQueryable<Ride> ReturningRidesFunction();

        private async ValueTask<Ride> TryCatch(ReturningRideFunction returningRideFunction)
        {
            try
            {
                return await returningRideFunction();
            }
            catch (NullRideException nullRideException)
            {
                throw CreateAndLogValidationException(nullRideException);
            }
            catch (InvalidRideException invalidRideException)
            {
                throw CreateAndLogValidationException(invalidRideException);
            }
            catch (NotFoundRideException nullRideException)
            {
                throw CreateAndLogValidationException(nullRideException);
            }
            catch (SqlException sqlException)
            {
                var failedRideStorageException =
                    new FailedRideStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedRideStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsRideException =
                    new AlreadyExistsRideException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsRideException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedRideException = new LockedRideException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedRideException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedRideStorageException =
                    new FailedRideStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedRideStorageException);
            }
            catch (Exception exception)
            {
                var failedRideServiceException =
                    new FailedRideServiceException(exception);

                throw CreateAndLogServiceException(failedRideServiceException);
            }
        }

        private IQueryable<Ride> TryCatch(ReturningRidesFunction returningRidesFunction)
        {
            try
            {
                return returningRidesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedRideStorageException =
                     new FailedRideStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedRideStorageException);
            }
            catch (Exception exception)
            {
                var failedRideServiceException =
                    new FailedRideServiceException(exception);

                throw CreateAndLogServiceException(failedRideServiceException);
            }
        }

        private RideValidationException CreateAndLogValidationException(Xeption exception)
        {
            var studentValidationException = new RideValidationException(exception);
            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }
        private RideDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)

        {
            var studentDependencyValidationException =
                new RideDependencyValidationException(exception);

            this.loggingBroker.LogError(studentDependencyValidationException);

            return studentDependencyValidationException;
        }

        private RideDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var studentDependencyException = new RideDependencyException(exception);
            this.loggingBroker.LogError(studentDependencyException);

            return studentDependencyException;
        }

        private RideDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var studentDependencyException = new RideDependencyException(exception);
            this.loggingBroker.LogCritical(studentDependencyException);

            return studentDependencyException;
        }

        private RideServiceException CreateAndLogServiceException(Xeption exception)
        {
            var studentServiceException = new RideServiceException(exception);
            this.loggingBroker.LogError(studentServiceException);

            return studentServiceException;
        }
    }
}
