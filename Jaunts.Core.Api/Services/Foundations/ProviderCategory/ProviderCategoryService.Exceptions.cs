// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategory;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategorys.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace Jaunts.Core.Api.Services.Foundations.ProviderCategories
{
    public partial class ProviderCategoryService
    {
        private delegate ValueTask<ProviderCategory> ReturningProviderCategoryFunction();
        private delegate IQueryable<ProviderCategory> ReturningProviderCategoriesFunction();

        private async ValueTask<ProviderCategory> TryCatch(ReturningProviderCategoryFunction returningProviderCategoryFunction)
        {
            try
            {
                return await returningProviderCategoryFunction();
            }
            catch (NullProviderCategoryException nullProviderCategoryException)
            {
                throw CreateAndLogValidationException(nullProviderCategoryException);
            }
            catch (InvalidProviderCategoryException invalidProviderCategoryException)
            {
                throw CreateAndLogValidationException(invalidProviderCategoryException);
            }
            catch (NotFoundProviderCategoryException nullProviderCategoryException)
            {
                throw CreateAndLogValidationException(nullProviderCategoryException);
            }
            catch (SqlException sqlException)
            {
                var failedProviderCategoryStorageException =
                    new FailedProviderCategoryStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedProviderCategoryStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsProviderCategoryException =
                    new AlreadyExistsProviderCategoryException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsProviderCategoryException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedProviderCategoryException = new LockedProviderCategoryException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedProviderCategoryException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedProviderCategoryStorageException =
                    new FailedProviderCategoryStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedProviderCategoryStorageException);
            }
            catch (Exception exception)
            {
                var failedProviderCategoryServiceException =
                    new FailedProviderCategoryServiceException(exception);

                throw CreateAndLogServiceException(failedProviderCategoryServiceException);
            }
        }

        private IQueryable<ProviderCategory> TryCatch(ReturningProviderCategoriesFunction returningProviderCategoriesFunction)
        {
            try
            {
                return returningProviderCategoriesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedProviderCategoryStorageException =
                     new FailedProviderCategoryStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedProviderCategoryStorageException);
            }
            catch (Exception exception)
            {
                var failedProviderCategoryServiceException =
                    new FailedProviderCategoryServiceException(exception);

                throw CreateAndLogServiceException(failedProviderCategoryServiceException);
            }
        }

        private ProviderCategoryValidationException CreateAndLogValidationException(Xeption exception)
        {
            var studentValidationException = new ProviderCategoryValidationException(exception);
            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }
        private ProviderCategoryDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)

        {
            var studentDependencyValidationException =
                new ProviderCategoryDependencyValidationException(exception);

            this.loggingBroker.LogError(studentDependencyValidationException);

            return studentDependencyValidationException;
        }

        private ProviderCategoryDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var studentDependencyException = new ProviderCategoryDependencyException(exception);
            this.loggingBroker.LogError(studentDependencyException);

            return studentDependencyException;
        }

        private ProviderCategoryDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var studentDependencyException = new ProviderCategoryDependencyException(exception);
            this.loggingBroker.LogCritical(studentDependencyException);

            return studentDependencyException;
        }

        private ProviderCategoryServiceException CreateAndLogServiceException(Xeption exception)
        {
            var studentServiceException = new ProviderCategoryServiceException(exception);
            this.loggingBroker.LogError(studentServiceException);

            return studentServiceException;
        }
    }
}
