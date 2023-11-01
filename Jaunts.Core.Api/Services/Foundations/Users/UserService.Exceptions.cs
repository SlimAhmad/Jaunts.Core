using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.User.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace Jaunts.Core.Api.Services.Foundations.Users
{
    public partial class UserService
    {
        private delegate ValueTask<ApplicationUser> ReturningUserFunction();
        private delegate IQueryable<ApplicationUser> ReturningQueryableUserFunction();


        private async ValueTask<ApplicationUser> TryCatch(ReturningUserFunction returningUserFunction)
        {
            try
            {
                return await returningUserFunction();
            }
            catch (NullUserException nullUserException)
            {
                throw CreateAndLogValidationException(nullUserException);
            }
            catch (InvalidUserException invalidUserException)
            {
                throw CreateAndLogValidationException(invalidUserException);
            }
            catch (SqlException sqlException)
            {
                var failedUserStorageException =
                    new FailedUserStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedUserStorageException);
            }
            catch (NotFoundUserException notFoundUserException)
            {
                throw CreateAndLogValidationException(notFoundUserException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsUserException =
                    new AlreadyExistsUserException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsUserException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidUserReferenceException =
                    new InvalidUserReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidUserReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedUserException = new LockedUserException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedUserException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedUserStorageException =
                    new FailedUserStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedUserStorageException);
            }
            catch (Exception exception)
            {
                var failedUserServiceException =
                    new FailedUserServiceException(exception);

                throw CreateAndLogServiceException(failedUserServiceException);
            }
        }

        private IQueryable<ApplicationUser> TryCatch(ReturningQueryableUserFunction  returningQueryableUserFunction)
        {
            try
            {
                return returningQueryableUserFunction();
            }
            catch (SqlException sqlException)
            {
                var failedUserStorageException =
                    new FailedUserStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedUserStorageException);
            }
            catch (Exception exception)
            {
                var failedUserServiceException =
                    new FailedUserServiceException(exception);

                throw CreateAndLogServiceException(failedUserServiceException);
            }
        }


        private UserValidationException CreateAndLogValidationException(
             Xeption exception)
        {
            var UserValidationException =
                new UserValidationException(exception);

            this.loggingBroker.LogError(UserValidationException);

            return UserValidationException;
        }

        private UserDependencyException CreateAndLogCriticalDependencyException(
            Xeption exception)
        {
            var UserDependencyException = new UserDependencyException(exception);
            this.loggingBroker.LogCritical(UserDependencyException);

            return UserDependencyException;
        }

        private UserDependencyValidationException CreateAndLogDependencyValidationException(
        Xeption exception)
        {
            var UserDependencyValidationException =
                new UserDependencyValidationException(exception);

            this.loggingBroker.LogError(UserDependencyValidationException);

            return UserDependencyValidationException;
        }

        private UserDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var UserDependencyException = new UserDependencyException(exception);
            this.loggingBroker.LogError(UserDependencyException);

            return UserDependencyException;
        }

        private UserServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var UserServiceException = new UserServiceException(exception);
            this.loggingBroker.LogError(UserServiceException);

            return UserServiceException;
        }

        private UserServiceException CreateAndLogServiceException(
            Exception exception)
        {
            var UserServiceException = new UserServiceException(exception);
            this.loggingBroker.LogError(UserServiceException);

            return UserServiceException;
        }
    }
}
