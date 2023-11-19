using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Processings.User.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.User.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using Xeptions;

namespace Jaunts.Core.Api.Services.Processings.User
{

    public partial class UserProcessingService
    {


        private delegate ValueTask<ApplicationUser> ReturningUserFunction();
        private delegate ValueTask<string> ReturningStringFunction();
        private delegate ValueTask<List<string>> ReturningListStringsFunction();
        private delegate ValueTask<bool> ReturningBooleanFunction();
        private delegate IQueryable<ApplicationUser> ReturningQueryableUserFunction();


        private async ValueTask<ApplicationUser> TryCatch(ReturningUserFunction returningUserFunction)
        {
            try
            {
                return await returningUserFunction();
            }
            catch (NullUserProcessingException nullUserProcessingException)
            {
                throw CreateAndLogValidationException(nullUserProcessingException);
            }
            catch (InvalidUserProcessingException invalidUserProcessingException)
            {
                throw CreateAndLogValidationException(invalidUserProcessingException);
            }
            catch (UserValidationException userValidationException)
            {
                throw CreateAndLogDependencyValidationException(userValidationException);
            }
            catch (UserDependencyValidationException userDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(userDependencyValidationException);
            }
            catch (UserDependencyException userDependencyException)
            {
                throw CreateAndLogDependencyException(userDependencyException);
            }
            catch (UserServiceException userServiceException)
            {
                throw CreateAndLogDependencyException(userServiceException);
            }
            catch (Exception exception)
            {
                var failedUserProcessingServiceException =
                    new FailedUserProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedUserProcessingServiceException);
            }
        }
        private async ValueTask<string> TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
            }
            catch (NullUserProcessingException nullUserProcessingException)
            {
                throw CreateAndLogValidationException(nullUserProcessingException);
            }
            catch (InvalidUserProcessingException invalidUserProcessingException)
            {
                throw CreateAndLogValidationException(invalidUserProcessingException);
            }
            catch (UserValidationException userValidationException)
            {
                throw CreateAndLogDependencyValidationException(userValidationException);
            }
            catch (UserDependencyValidationException userDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(userDependencyValidationException);
            }
            catch (UserDependencyException userDependencyException)
            {
                throw CreateAndLogDependencyException(userDependencyException);
            }
            catch (UserServiceException userServiceException)
            {
                throw CreateAndLogDependencyException(userServiceException);
            }
            catch (Exception exception)
            {
                var failedUserProcessingServiceException =
                    new FailedUserProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedUserProcessingServiceException);
            }
        }
        private async ValueTask<List<string>> TryCatch(ReturningListStringsFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
            }
            catch (NullUserProcessingException nullUserProcessingException)
            {
                throw CreateAndLogValidationException(nullUserProcessingException);
            }
            catch (InvalidUserProcessingException invalidUserProcessingException)
            {
                throw CreateAndLogValidationException(invalidUserProcessingException);
            }
            catch (UserValidationException userValidationException)
            {
                throw CreateAndLogDependencyValidationException(userValidationException);
            }
            catch (UserDependencyValidationException userDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(userDependencyValidationException);
            }
            catch (UserDependencyException userDependencyException)
            {
                throw CreateAndLogDependencyException(userDependencyException);
            }
            catch (UserServiceException userServiceException)
            {
                throw CreateAndLogDependencyException(userServiceException);
            }
            catch (Exception exception)
            {
                var failedUserProcessingServiceException =
                    new FailedUserProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedUserProcessingServiceException);
            }
        }
        private async ValueTask<bool> TryCatch(ReturningBooleanFunction returningBooleanFunction)
        {
            try
            {
                return await returningBooleanFunction();
            }
            catch (NullUserProcessingException nullUserProcessingException)
            {
                throw CreateAndLogValidationException(nullUserProcessingException);
            }
            catch (InvalidUserProcessingException invalidUserProcessingException)
            {
                throw CreateAndLogValidationException(invalidUserProcessingException);
            }
            catch (UserValidationException userValidationException)
            {
                throw CreateAndLogDependencyValidationException(userValidationException);
            }
            catch (UserDependencyValidationException userDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(userDependencyValidationException);
            }
            catch (UserDependencyException userDependencyException)
            {
                throw CreateAndLogDependencyException(userDependencyException);
            }
            catch (UserServiceException userServiceException)
            {
                throw CreateAndLogDependencyException(userServiceException);
            }
            catch (Exception exception)
            {
                var failedUserProcessingServiceException =
                    new FailedUserProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedUserProcessingServiceException);
            }
        }
        private IQueryable<ApplicationUser> TryCatch(ReturningQueryableUserFunction returningQueryableUserFunction)
        {
            try
            {
                return returningQueryableUserFunction();
            }
            catch (NullUserProcessingException nullUserProcessingException)
            {
                throw CreateAndLogValidationException(nullUserProcessingException);
            }
            catch (InvalidUserProcessingException invalidUserProcessingException)
            {
                throw CreateAndLogValidationException(invalidUserProcessingException);
            }
            catch (UserValidationException userValidationException)
            {
                throw CreateAndLogDependencyValidationException(userValidationException);
            }
            catch (UserDependencyValidationException userDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(userDependencyValidationException);
            }
            catch (UserDependencyException userDependencyException)
            {
                throw CreateAndLogDependencyException(userDependencyException);
            }
            catch (UserServiceException userServiceException)
            {
                throw CreateAndLogDependencyException(userServiceException);
            }
            catch (Exception exception)
            {
                var failedUserProcessingServiceException =
                    new FailedUserProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedUserProcessingServiceException);
            }
        }


        private UserProcessingDependencyException CreateAndLogCriticalDependencyException(
          Xeption exception)
        {
            var UserProcessingDependencyException =
                new UserProcessingDependencyException(exception);

            this.loggingBroker.LogCritical(UserProcessingDependencyException);

            return UserProcessingDependencyException;
        }
        private UserProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var userProcessingServiceException = new
                UserProcessingServiceException(exception);

            this.loggingBroker.LogError(userProcessingServiceException);

            return userProcessingServiceException;
        }

        private UserProcessingDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var userProcessingDependencyValidationException =
                new UserProcessingDependencyValidationException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(userProcessingDependencyValidationException);

            return userProcessingDependencyValidationException;
        }

        private UserProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var userProcessingDependencyException =
                new UserProcessingDependencyException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(userProcessingDependencyException);

            return userProcessingDependencyException;
        }

        private UserProcessingValidationException CreateAndLogValidationException(
            Xeption exception)
        {
            var userProcessingValidationException =
                new UserProcessingValidationException(exception);

            this.loggingBroker.LogError(userProcessingValidationException);

            return userProcessingValidationException;
        }

    }
}