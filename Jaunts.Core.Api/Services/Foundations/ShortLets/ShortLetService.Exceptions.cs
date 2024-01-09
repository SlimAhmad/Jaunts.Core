// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLets;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLets.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace Jaunts.Core.Api.Services.Foundations.ShortLets
{
    public partial class ShortLetService
    {
        private delegate ValueTask<ShortLet> ReturningShortLetFunction();
        private delegate IQueryable<ShortLet> ReturningShortLetsFunction();

        private async ValueTask<ShortLet> TryCatch(ReturningShortLetFunction returningShortLetFunction)
        {
            try
            {
                return await returningShortLetFunction();
            }
            catch (NullShortLetException nullShortLetException)
            {
                throw CreateAndLogValidationException(nullShortLetException);
            }
            catch (InvalidShortLetException invalidShortLetException)
            {
                throw CreateAndLogValidationException(invalidShortLetException);
            }
            catch (NotFoundShortLetException nullShortLetException)
            {
                throw CreateAndLogValidationException(nullShortLetException);
            }
            catch (SqlException sqlException)
            {
                var failedShortLetStorageException =
                    new FailedShortLetStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedShortLetStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsShortLetException =
                    new AlreadyExistsShortLetException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsShortLetException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedShortLetException = new LockedShortLetException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedShortLetException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedShortLetStorageException =
                    new FailedShortLetStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedShortLetStorageException);
            }
            catch (Exception exception)
            {
                var failedShortLetServiceException =
                    new FailedShortLetServiceException(exception);

                throw CreateAndLogServiceException(failedShortLetServiceException);
            }
        }

        private IQueryable<ShortLet> TryCatch(ReturningShortLetsFunction returningShortLetsFunction)
        {
            try
            {
                return returningShortLetsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedShortLetStorageException =
                     new FailedShortLetStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedShortLetStorageException);
            }
            catch (Exception exception)
            {
                var failedShortLetServiceException =
                    new FailedShortLetServiceException(exception);

                throw CreateAndLogServiceException(failedShortLetServiceException);
            }
        }

        private ShortLetValidationException CreateAndLogValidationException(Xeption exception)
        {
            var studentValidationException = new ShortLetValidationException(exception);
            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }
        private ShortLetDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)

        {
            var studentDependencyValidationException =
                new ShortLetDependencyValidationException(exception);

            this.loggingBroker.LogError(studentDependencyValidationException);

            return studentDependencyValidationException;
        }

        private ShortLetDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var studentDependencyException = new ShortLetDependencyException(exception);
            this.loggingBroker.LogError(studentDependencyException);

            return studentDependencyException;
        }

        private ShortLetDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var studentDependencyException = new ShortLetDependencyException(exception);
            this.loggingBroker.LogCritical(studentDependencyException);

            return studentDependencyException;
        }

        private ShortLetServiceException CreateAndLogServiceException(Xeption exception)
        {
            var studentServiceException = new ShortLetServiceException(exception);
            this.loggingBroker.LogError(studentServiceException);

            return studentServiceException;
        }
    }
}
