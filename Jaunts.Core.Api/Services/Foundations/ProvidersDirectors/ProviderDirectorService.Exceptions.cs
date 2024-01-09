// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace Jaunts.Core.Api.Services.Foundations.ProvidersDirectors
{
    public partial class ProvidersDirectorService
    {
        private delegate ValueTask<ProvidersDirector> ReturningProvidersDirectorFunction();
        private delegate IQueryable<ProvidersDirector> ReturningProvidersDirectorCategoriesFunction();

        private async ValueTask<ProvidersDirector> TryCatch(ReturningProvidersDirectorFunction returningProvidersDirectorFunction)
        {
            try
            {
                return await returningProvidersDirectorFunction();
            }
            catch (NullProvidersDirectorException nullProvidersDirectorException)
            {
                throw CreateAndLogValidationException(nullProvidersDirectorException);
            }
            catch (InvalidProvidersDirectorException invalidProvidersDirectorException)
            {
                throw CreateAndLogValidationException(invalidProvidersDirectorException);
            }
            catch (NotFoundProvidersDirectorException nullProvidersDirectorException)
            {
                throw CreateAndLogValidationException(nullProvidersDirectorException);
            }
            catch (SqlException sqlException)
            {
                var failedProvidersDirectorStorageException =
                    new FailedProvidersDirectorStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedProvidersDirectorStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsProvidersDirectorException =
                    new AlreadyExistsProvidersDirectorException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsProvidersDirectorException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedProvidersDirectorException = new LockedProvidersDirectorException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedProvidersDirectorException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedProvidersDirectorStorageException =
                    new FailedProvidersDirectorStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedProvidersDirectorStorageException);
            }
            catch (Exception exception)
            {
                var failedProvidersDirectorServiceException =
                    new FailedProvidersDirectorServiceException(exception);

                throw CreateAndLogServiceException(failedProvidersDirectorServiceException);
            }
        }

        private IQueryable<ProvidersDirector> TryCatch(ReturningProvidersDirectorCategoriesFunction returningProvidersDirectorCategoriesFunction)
        {
            try
            {
                return returningProvidersDirectorCategoriesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedProvidersDirectorStorageException =
                     new FailedProvidersDirectorStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedProvidersDirectorStorageException);
            }
            catch (Exception exception)
            {
                var failedProvidersDirectorServiceException =
                    new FailedProvidersDirectorServiceException(exception);

                throw CreateAndLogServiceException(failedProvidersDirectorServiceException);
            }
        }

        private ProvidersDirectorValidationException CreateAndLogValidationException(Xeption exception)
        {
            var studentValidationException = new ProvidersDirectorValidationException(exception);
            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }
        private ProvidersDirectorDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)

        {
            var studentDependencyValidationException =
                new ProvidersDirectorDependencyValidationException(exception);

            this.loggingBroker.LogError(studentDependencyValidationException);

            return studentDependencyValidationException;
        }

        private ProvidersDirectorDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var studentDependencyException = new ProvidersDirectorDependencyException(exception);
            this.loggingBroker.LogError(studentDependencyException);

            return studentDependencyException;
        }

        private ProvidersDirectorDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var studentDependencyException = new ProvidersDirectorDependencyException(exception);
            this.loggingBroker.LogCritical(studentDependencyException);

            return studentDependencyException;
        }

        private ProvidersDirectorServiceException CreateAndLogServiceException(Xeption exception)
        {
            var studentServiceException = new ProvidersDirectorServiceException(exception);
            this.loggingBroker.LogError(studentServiceException);

            return studentServiceException;
        }
    }
}
