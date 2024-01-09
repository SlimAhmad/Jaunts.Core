// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace Jaunts.Core.Api.Services.Foundations.FlightDeals
{
    public partial class FlightDealService
    {
        private delegate ValueTask<FlightDeal> ReturningFlightDealFunction();
        private delegate IQueryable<FlightDeal> ReturningFlightDealsFunction();

        private async ValueTask<FlightDeal> TryCatch(ReturningFlightDealFunction returningFlightDealFunction)
        {
            try
            {
                return await returningFlightDealFunction();
            }
            catch (NullFlightDealException nullFlightDealException)
            {
                throw CreateAndLogValidationException(nullFlightDealException);
            }
            catch (InvalidFlightDealException invalidFlightDealException)
            {
                throw CreateAndLogValidationException(invalidFlightDealException);
            }
            catch (NotFoundFlightDealException nullFlightDealException)
            {
                throw CreateAndLogValidationException(nullFlightDealException);
            }
            catch (SqlException sqlException)
            {
                var failedFlightDealStorageException =
                    new FailedFlightDealStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedFlightDealStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsFlightDealException =
                    new AlreadyExistsFlightDealException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsFlightDealException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedFlightDealException = new LockedFlightDealException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedFlightDealException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedFlightDealStorageException =
                    new FailedFlightDealStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedFlightDealStorageException);
            }
            catch (Exception exception)
            {
                var failedFlightDealServiceException =
                    new FailedFlightDealServiceException(exception);

                throw CreateAndLogServiceException(failedFlightDealServiceException);
            }
        }

        private IQueryable<FlightDeal> TryCatch(ReturningFlightDealsFunction returningFlightDealsFunction)
        {
            try
            {
                return returningFlightDealsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedFlightDealStorageException =
                     new FailedFlightDealStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedFlightDealStorageException);
            }
            catch (Exception exception)
            {
                var failedFlightDealServiceException =
                    new FailedFlightDealServiceException(exception);

                throw CreateAndLogServiceException(failedFlightDealServiceException);
            }
        }

        private FlightDealValidationException CreateAndLogValidationException(Xeption exception)
        {
            var studentValidationException = new FlightDealValidationException(exception);
            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }
        private FlightDealDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)

        {
            var studentDependencyValidationException =
                new FlightDealDependencyValidationException(exception);

            this.loggingBroker.LogError(studentDependencyValidationException);

            return studentDependencyValidationException;
        }

        private FlightDealDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var studentDependencyException = new FlightDealDependencyException(exception);
            this.loggingBroker.LogError(studentDependencyException);

            return studentDependencyException;
        }

        private FlightDealDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var studentDependencyException = new FlightDealDependencyException(exception);
            this.loggingBroker.LogCritical(studentDependencyException);

            return studentDependencyException;
        }

        private FlightDealServiceException CreateAndLogServiceException(Xeption exception)
        {
            var studentServiceException = new FlightDealServiceException(exception);
            this.loggingBroker.LogError(studentServiceException);

            return studentServiceException;
        }
    }
}
