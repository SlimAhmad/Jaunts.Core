// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Adverts;
using Jaunts.Core.Api.Models.Services.Foundations.Adverts.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace Jaunts.Core.Api.Services.Foundations.Adverts
{
    public partial class AdvertService
    {
        private delegate ValueTask<Advert> ReturningAdvertFunction();
        private delegate IQueryable<Advert> ReturningAdvertsFunction();

        private async ValueTask<Advert> TryCatch(ReturningAdvertFunction returningAdvertFunction)
        {
            try
            {
                return await returningAdvertFunction();
            }
            catch (NullAdvertException nullAdvertException)
            {
                throw CreateAndLogValidationException(nullAdvertException);
            }
            catch (InvalidAdvertException invalidAdvertException)
            {
                throw CreateAndLogValidationException(invalidAdvertException);
            }
            catch (NotFoundAdvertException nullAdvertException)
            {
                throw CreateAndLogValidationException(nullAdvertException);
            }
            catch (SqlException sqlException)
            {
                var failedAdvertStorageException =
                    new FailedAdvertStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedAdvertStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsAdvertException =
                    new AlreadyExistsAdvertException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsAdvertException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedAdvertException = new LockedAdvertException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedAdvertException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedAdvertStorageException =
                    new FailedAdvertStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedAdvertStorageException);
            }
            catch (Exception exception)
            {
                var failedAdvertServiceException =
                    new FailedAdvertServiceException(exception);

                throw CreateAndLogServiceException(failedAdvertServiceException);
            }
        }

        private IQueryable<Advert> TryCatch(ReturningAdvertsFunction returningAdvertsFunction)
        {
            try
            {
                return returningAdvertsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedAdvertStorageException =
                     new FailedAdvertStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedAdvertStorageException);
            }
            catch (Exception exception)
            {
                var failedAdvertServiceException =
                    new FailedAdvertServiceException(exception);

                throw CreateAndLogServiceException(failedAdvertServiceException);
            }
        }

        private AdvertValidationException CreateAndLogValidationException(Xeption exception)
        {
            var studentValidationException = new AdvertValidationException(exception);
            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }
        private AdvertDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)

        {
            var studentDependencyValidationException =
                new AdvertDependencyValidationException(exception);

            this.loggingBroker.LogError(studentDependencyValidationException);

            return studentDependencyValidationException;
        }

        private AdvertDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var studentDependencyException = new AdvertDependencyException(exception);
            this.loggingBroker.LogError(studentDependencyException);

            return studentDependencyException;
        }

        private AdvertDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var studentDependencyException = new AdvertDependencyException(exception);
            this.loggingBroker.LogCritical(studentDependencyException);

            return studentDependencyException;
        }

        private AdvertServiceException CreateAndLogServiceException(Xeption exception)
        {
            var studentServiceException = new AdvertServiceException(exception);
            this.loggingBroker.LogError(studentServiceException);

            return studentServiceException;
        }
    }
}
