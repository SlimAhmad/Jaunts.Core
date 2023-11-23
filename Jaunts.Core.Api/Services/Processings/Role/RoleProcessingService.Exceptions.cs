using Jaunts.Core.Api.Models.Processings.Role.Exceptions;
using Jaunts.Core.Api.Models.Role.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using RESTFulSense.Exceptions;
using Xeptions;

namespace Jaunts.Core.Api.Services.Processings.Role
{

    public partial class RoleProcessingService
    {
       

        private delegate ValueTask<int> ReturningPermissionsFunction();
        private delegate ValueTask<bool> ReturningBooleanFunction();
        private delegate ValueTask<ApplicationRole> ReturningRoleFunction();
        private delegate IQueryable<ApplicationRole> ReturningQueryableRoleFunction();

        private async ValueTask<int> TryCatch(ReturningPermissionsFunction returningPermissionsFunction)
        {
            try
            {
                return await returningPermissionsFunction();
            }
            catch (NullRoleProcessingException nullRoleProcessingException)
            {
                throw CreateAndLogValidationException(nullRoleProcessingException);
            }
            catch (InvalidRoleProcessingException invalidRoleProcessingException)
            {
                throw CreateAndLogValidationException(invalidRoleProcessingException);
            }
            catch (RoleValidationException roleValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleValidationException);
            }
            catch (RoleDependencyValidationException roleDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleDependencyValidationException);
            }
            catch (RoleDependencyException roleDependencyException)
            {
                throw CreateAndLogDependencyException(roleDependencyException);
            }
            catch (RoleServiceException roleServiceException)
            {
                throw CreateAndLogDependencyException(roleServiceException);
            }
            catch (Exception exception)
            {
                var failedRoleProcessingServiceException =
                    new FailedRoleProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedRoleProcessingServiceException);
            }
        }
        private async ValueTask<bool> TryCatch(ReturningBooleanFunction returningBooleanFunction)
        {
            try
            {
                return await returningBooleanFunction();
            }
            catch (NullRoleProcessingException nullRoleProcessingException)
            {
                throw CreateAndLogValidationException(nullRoleProcessingException);
            }
            catch (InvalidRoleProcessingException invalidRoleProcessingException)
            {
                throw CreateAndLogValidationException(invalidRoleProcessingException);
            }
            catch (RoleValidationException roleValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleValidationException);
            }
            catch (RoleDependencyValidationException roleDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleDependencyValidationException);
            }
            catch (RoleDependencyException roleDependencyException)
            {
                throw CreateAndLogDependencyException(roleDependencyException);
            }
            catch (RoleServiceException roleServiceException)
            {
                throw CreateAndLogDependencyException(roleServiceException);
            }
            catch (Exception exception)
            {
                var failedRoleProcessingServiceException =
                    new FailedRoleProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedRoleProcessingServiceException);
            }
        }
        private async ValueTask<ApplicationRole> TryCatch(ReturningRoleFunction returningRoleFunction)
        {
            try
            {
                return await returningRoleFunction();
            }
            catch (NullRoleProcessingException nullRoleProcessingException)
            {
                throw CreateAndLogValidationException(nullRoleProcessingException);
            }
            catch (InvalidRoleProcessingException invalidRoleProcessingException)
            {
                throw CreateAndLogValidationException(invalidRoleProcessingException);
            }
            catch (RoleValidationException roleValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleValidationException);
            }
            catch (RoleDependencyValidationException roleDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleDependencyValidationException);
            }
            catch (RoleDependencyException roleDependencyException)
            {
                throw CreateAndLogDependencyException(roleDependencyException);
            }
            catch (RoleServiceException roleServiceException)
            {
                throw CreateAndLogDependencyException(roleServiceException);
            }
            catch (Exception exception)
            {
                var failedRoleProcessingServiceException =
                    new FailedRoleProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedRoleProcessingServiceException);
            }
        }
        private IQueryable<ApplicationRole> TryCatch(ReturningQueryableRoleFunction returningQueryableRoleFunction)
        {
            try
            {
                return returningQueryableRoleFunction();
            }
            catch (NullRoleProcessingException nullRoleProcessingException)
            {
                throw CreateAndLogValidationException(nullRoleProcessingException);
            }
            catch (InvalidRoleProcessingException invalidRoleProcessingException)
            {
                throw CreateAndLogValidationException(invalidRoleProcessingException);
            }
            catch (RoleValidationException roleValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleValidationException);
            }
            catch (RoleDependencyValidationException roleDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleDependencyValidationException);
            }
            catch (RoleDependencyException roleDependencyException)
            {
                throw CreateAndLogDependencyException(roleDependencyException);
            }
            catch (RoleServiceException roleServiceException)
            {
                throw CreateAndLogDependencyException(roleServiceException);
            }
            catch (Exception exception)
            {
                var failedRoleProcessingServiceException =
                    new FailedRoleProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedRoleProcessingServiceException);
            }
        }


        private RoleProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var roleProcessingServiceException = new
                RoleProcessingServiceException(exception);

            this.loggingBroker.LogError(roleProcessingServiceException);

            return roleProcessingServiceException;
        }

        private RoleProcessingDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var roleProcessingDependencyValidationException =
                new RoleProcessingDependencyValidationException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(roleProcessingDependencyValidationException);

            return roleProcessingDependencyValidationException;
        }

        private RoleProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var roleProcessingDependencyException =
                new RoleProcessingDependencyException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(roleProcessingDependencyException);

            return roleProcessingDependencyException;
        }

        private RoleProcessingValidationException CreateAndLogValidationException(
            Xeption exception)
        {
            var roleProcessingValidationException =
                new RoleProcessingValidationException(exception);

            this.loggingBroker.LogError(roleProcessingValidationException);

            return roleProcessingValidationException;
        }
    }
}