// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderServices;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderServices.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace Jaunts.Core.Api.Services.Foundations.ProviderServices
{
    public partial class ProviderServiceService
    {
        private delegate ValueTask<ProviderService> ReturningProviderServiceFunction();
        private delegate IQueryable<ProviderService> ReturningProviderServiceCategoriesFunction();

        private async ValueTask<ProviderService> TryCatch(ReturningProviderServiceFunction returningProviderServiceFunction)
        {
            try
            {
                return await returningProviderServiceFunction();
            }
            catch (NullProviderServiceException nullProviderServiceException)
            {
                throw CreateAndLogValidationException(nullProviderServiceException);
            }
            catch (InvalidProviderServiceException invalidProviderServiceException)
            {
                throw CreateAndLogValidationException(invalidProviderServiceException);
            }
            catch (NotFoundProviderServiceException nullProviderServiceException)
            {
                throw CreateAndLogValidationException(nullProviderServiceException);
            }
            catch (SqlException sqlException)
            {
                var failedProviderServiceStorageException =
                    new FailedProviderServiceStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedProviderServiceStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsProviderServiceException =
                    new AlreadyExistsProviderServiceException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsProviderServiceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedProviderServiceException = new LockedProviderServiceException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedProviderServiceException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedProviderServiceStorageException =
                    new FailedProviderServiceStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedProviderServiceStorageException);
            }
            catch (Exception exception)
            {
                var failedProviderServiceServiceException =
                    new FailedProviderServiceServiceException(exception);

                throw CreateAndLogServiceException(failedProviderServiceServiceException);
            }
        }

        private IQueryable<ProviderService> TryCatch(ReturningProviderServiceCategoriesFunction returningProviderServiceCategoriesFunction)
        {
            try
            {
                return returningProviderServiceCategoriesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedProviderServiceStorageException =
                     new FailedProviderServiceStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedProviderServiceStorageException);
            }
            catch (Exception exception)
            {
                var failedProviderServiceServiceException =
                    new FailedProviderServiceServiceException(exception);

                throw CreateAndLogServiceException(failedProviderServiceServiceException);
            }
        }

        private ProviderServiceValidationException CreateAndLogValidationException(Xeption exception)
        {
            var studentValidationException = new ProviderServiceValidationException(exception);
            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }
        private ProviderServiceDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)

        {
            var studentDependencyValidationException =
                new ProviderServiceDependencyValidationException(exception);

            this.loggingBroker.LogError(studentDependencyValidationException);

            return studentDependencyValidationException;
        }

        private ProviderServiceDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var studentDependencyException = new ProviderServiceDependencyException(exception);
            this.loggingBroker.LogError(studentDependencyException);

            return studentDependencyException;
        }

        private ProviderServiceDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var studentDependencyException = new ProviderServiceDependencyException(exception);
            this.loggingBroker.LogCritical(studentDependencyException);

            return studentDependencyException;
        }

        private ProviderServiceServiceException CreateAndLogServiceException(Xeption exception)
        {
            var studentServiceException = new ProviderServiceServiceException(exception);
            this.loggingBroker.LogError(studentServiceException);

            return studentServiceException;
        }
    }
}
