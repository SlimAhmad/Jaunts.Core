// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Providers;
using Jaunts.Core.Api.Models.Services.Foundations.Providers.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace Jaunts.Core.Api.Services.Foundations.Providers
{
    public partial class ProviderService
    {
        private delegate ValueTask<Provider> ReturningProviderFunction();
        private delegate IQueryable<Provider> ReturningProviderCategoriesFunction();

        private async ValueTask<Provider> TryCatch(ReturningProviderFunction returningProviderFunction)
        {
            try
            {
                return await returningProviderFunction();
            }
            catch (NullProviderException nullProviderException)
            {
                throw CreateAndLogValidationException(nullProviderException);
            }
            catch (InvalidProviderException invalidProviderException)
            {
                throw CreateAndLogValidationException(invalidProviderException);
            }
            catch (NotFoundProviderException nullProviderException)
            {
                throw CreateAndLogValidationException(nullProviderException);
            }
            catch (SqlException sqlException)
            {
                var failedProviderStorageException =
                    new FailedProviderStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedProviderStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsProviderException =
                    new AlreadyExistsProviderException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsProviderException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedProviderException = new LockedProviderException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedProviderException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedProviderStorageException =
                    new FailedProviderStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedProviderStorageException);
            }
            catch (Exception exception)
            {
                var failedProviderServiceException =
                    new FailedProviderServiceException(exception);

                throw CreateAndLogServiceException(failedProviderServiceException);
            }
        }

        private IQueryable<Provider> TryCatch(ReturningProviderCategoriesFunction returningProviderCategoriesFunction)
        {
            try
            {
                return returningProviderCategoriesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedProviderStorageException =
                     new FailedProviderStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedProviderStorageException);
            }
            catch (Exception exception)
            {
                var failedProviderServiceException =
                    new FailedProviderServiceException(exception);

                throw CreateAndLogServiceException(failedProviderServiceException);
            }
        }

        private ProviderValidationException CreateAndLogValidationException(Xeption exception)
        {
            var studentValidationException = new ProviderValidationException(exception);
            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }
        private ProviderDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)

        {
            var studentDependencyValidationException =
                new ProviderDependencyValidationException(exception);

            this.loggingBroker.LogError(studentDependencyValidationException);

            return studentDependencyValidationException;
        }

        private ProviderDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var studentDependencyException = new ProviderDependencyException(exception);
            this.loggingBroker.LogError(studentDependencyException);

            return studentDependencyException;
        }

        private ProviderDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var studentDependencyException = new ProviderDependencyException(exception);
            this.loggingBroker.LogCritical(studentDependencyException);

            return studentDependencyException;
        }

        private ProviderServiceException CreateAndLogServiceException(Xeption exception)
        {
            var studentServiceException = new ProviderServiceException(exception);
            this.loggingBroker.LogError(studentServiceException);

            return studentServiceException;
        }
    }
}
