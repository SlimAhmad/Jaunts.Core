using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Processings.Role.Exceptions;
using Jaunts.Core.Api.Models.Role.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Auth.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Models.Services.Orchestration.Role.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace Jaunts.Core.Api.Services.Orchestration.Role
{
    public partial class RoleOrchestrationService
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
            catch (RoleProcessingValidationException roleValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleValidationException);
            }
            catch (RoleProcessingDependencyValidationException roleDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleDependencyValidationException);
            }
            catch (RoleProcessingDependencyException roleDependencyException)
            {
                throw CreateAndLogDependencyException(roleDependencyException);
            }
            catch (RoleProcessingServiceException roleServiceException)
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
            catch (RoleProcessingValidationException roleValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleValidationException);
            }
            catch (RoleProcessingDependencyValidationException roleDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleDependencyValidationException);
            }
            catch (RoleProcessingDependencyException roleDependencyException)
            {
                throw CreateAndLogDependencyException(roleDependencyException);
            }
            catch (RoleProcessingServiceException roleServiceException)
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
            catch (RoleProcessingValidationException roleValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleValidationException);
            }
            catch (RoleProcessingDependencyValidationException roleDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleDependencyValidationException);
            }
            catch (RoleProcessingDependencyException roleDependencyException)
            {
                throw CreateAndLogDependencyException(roleDependencyException);
            }
            catch (RoleProcessingServiceException roleServiceException)
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
            catch (RoleProcessingValidationException roleValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleValidationException);
            }
            catch (RoleProcessingDependencyValidationException roleDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleDependencyValidationException);
            }
            catch (RoleProcessingDependencyException roleDependencyException)
            {
                throw CreateAndLogDependencyException(roleDependencyException);
            }
            catch (RoleProcessingServiceException roleServiceException)
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


        private RoleOrchestrationServiceException CreateAndLogServiceException(Xeption exception)
        {
            var roleOrchestrationServiceException = new
                RoleOrchestrationServiceException(exception);

            this.loggingBroker.LogError(roleOrchestrationServiceException);

            return roleOrchestrationServiceException;
        }

        private RoleOrchestrationDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var roleOrchestrationDependencyValidationException =
                new RoleOrchestrationDependencyValidationException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(roleOrchestrationDependencyValidationException);

            return roleOrchestrationDependencyValidationException;
        }

        private RoleOrchestrationDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var roleOrchestrationDependencyException =
                new RoleOrchestrationDependencyException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(roleOrchestrationDependencyException);

            return roleOrchestrationDependencyException;
        }

      
    }
}
