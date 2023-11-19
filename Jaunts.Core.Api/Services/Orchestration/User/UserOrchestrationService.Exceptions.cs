using Jaunts.Core.Api.Models.Processings.Role.Exceptions;
using Jaunts.Core.Api.Models.Processings.User.Exceptions;
using Jaunts.Core.Api.Models.Role.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.Services.Orchestration.Role.Exceptions;
using Jaunts.Core.Api.Models.Services.Orchestration.User.Exceptions;
using Jaunts.Core.Api.Models.User.Exceptions;
using System.Diagnostics.Metrics;
using Xeptions;

namespace Jaunts.Core.Api.Services.Orchestration.User
{
    public partial class UserOrchestrationService
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


        private UserOrchestrationServiceException CreateAndLogServiceException(Xeption exception)
        {
            var userOrchestrationServiceException = new
               UserOrchestrationServiceException(exception);

            this.loggingBroker.LogError(userOrchestrationServiceException);

            return userOrchestrationServiceException;
        }

        private UserOrchestrationDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var userOrchestrationDependencyValidationException =
                new UserOrchestrationDependencyValidationException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(userOrchestrationDependencyValidationException);

            return userOrchestrationDependencyValidationException;
        }

        private UserOrchestrationDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var userOrchestrationDependencyException =
                new UserOrchestrationDependencyException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(userOrchestrationDependencyException);

            return userOrchestrationDependencyException;
        }


    }
}
