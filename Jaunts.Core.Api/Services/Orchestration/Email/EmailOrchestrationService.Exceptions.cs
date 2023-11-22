using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Auth.Exceptions;
using Jaunts.Core.Models.Auth.LoginRegister;
using Jaunts.Core.Models.Email;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace Jaunts.Core.Api.Services.Orchestration.Email
{
    public partial class EmailOrchestrationService
    {
  
        private delegate ValueTask<SendEmailResponse> ReturningEmailResponseFunction();
        private delegate ValueTask<UserAccountDetailsApiResponse> ReturningUserAccountDetailsFunction();

        private async ValueTask<UserAccountDetailsApiResponse> TryCatch(ReturningUserAccountDetailsFunction returningUserAccountDetailsFunction)
        {
            try
            {
                return await returningUserAccountDetailsFunction();
            }
            catch (NullAuthException nullAuthException)
            {
                throw CreateAndLogValidationException(nullAuthException);
            }
            catch (InvalidAuthException invalidAuthException)
            {
                throw CreateAndLogValidationException(invalidAuthException);
            }
            catch (SqlException sqlException)
            {
                var failedAuthStorageException =
                    new FailedAuthStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedAuthStorageException);
            }
            catch (NotFoundAuthException notFoundAuthException)
            {
                throw CreateAndLogValidationException(notFoundAuthException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsAuthException =
                    new AlreadyExistsAuthException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsAuthException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidAuthReferenceException =
                    new InvalidAuthReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidAuthReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedAuthException = new LockedAuthException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedAuthException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedAuthStorageException =
                    new FailedAuthStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedAuthStorageException);
            }
            catch (Exception exception)
            {
                var failedAuthServiceException =
                    new FailedAuthServiceException(exception);

                throw CreateAndLogServiceException(failedAuthServiceException);
            }
        }

        private async ValueTask<SendEmailResponse> TryCatch(ReturningEmailResponseFunction returningEmailResponseFunction)
        {
            try
            {
                return await returningEmailResponseFunction();
            }
            catch (NullAuthException nullAuthException)
            {
                throw CreateAndLogValidationException(nullAuthException);
            }
            catch (InvalidAuthException invalidAuthException)
            {
                throw CreateAndLogValidationException(invalidAuthException);
            }
            catch (SqlException sqlException)
            {
                var failedAuthStorageException =
                    new FailedAuthStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedAuthStorageException);
            }
            catch (NotFoundAuthException notFoundAuthException)
            {
                throw CreateAndLogValidationException(notFoundAuthException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsAuthException =
                    new AlreadyExistsAuthException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsAuthException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidAuthReferenceException =
                    new InvalidAuthReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidAuthReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedAuthException = new LockedAuthException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedAuthException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedAuthStorageException =
                    new FailedAuthStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedAuthStorageException);
            }
            catch (Exception exception)
            {
                var failedAuthServiceException =
                    new FailedAuthServiceException(exception);

                throw CreateAndLogServiceException(failedAuthServiceException);
            }
        }
     

        private AuthValidationException CreateAndLogValidationException(
             Xeption exception)
        {
            var AuthValidationException =
                new AuthValidationException(exception);

            this.loggingBroker.LogError(AuthValidationException);

            return AuthValidationException;
        }

        private AuthDependencyException CreateAndLogCriticalDependencyException(
            Xeption exception)
        {
            var AuthDependencyException = new AuthDependencyException(exception);
            this.loggingBroker.LogCritical(AuthDependencyException);

            return AuthDependencyException;
        }

        private AuthDependencyValidationException CreateAndLogDependencyValidationException(
        Xeption exception)
        {
            var AuthDependencyValidationException =
                new AuthDependencyValidationException(exception);

            this.loggingBroker.LogError(AuthDependencyValidationException);

            return AuthDependencyValidationException;
        }

        private AuthDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var AuthDependencyException = new AuthDependencyException(exception);
            this.loggingBroker.LogError(AuthDependencyException);

            return AuthDependencyException;
        }

        private AuthServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var AuthServiceException = new AuthServiceException(exception);
            this.loggingBroker.LogError(AuthServiceException);

            return AuthServiceException;
        }

        private AuthServiceException CreateAndLogServiceException(
            Exception exception)
        {
            var AuthServiceException = new AuthServiceException(exception);
            this.loggingBroker.LogError(AuthServiceException);

            return AuthServiceException;
        }
    }
}
