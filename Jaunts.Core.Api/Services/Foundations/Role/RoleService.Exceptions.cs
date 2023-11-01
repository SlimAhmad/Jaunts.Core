using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Role.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Auth.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace Jaunts.Core.Api.Services.Foundations.Role
{
    public partial class RoleService
    {
        private delegate ValueTask<ApplicationRole> ReturningRoleFunction();
        private delegate IQueryable<ApplicationRole> ReturningQueryableRoleFunction();


        private async ValueTask<ApplicationRole> TryCatch(ReturningRoleFunction returningRoleFunction)
        {
            try
            {
                return await returningRoleFunction();
            }
            catch (NullRoleException nullRoleException)
            {
                throw CreateAndLogValidationException(nullRoleException);
            }
            catch (InvalidRoleException invalidRoleException)
            {
                throw CreateAndLogValidationException(invalidRoleException);
            }
            catch (SqlException sqlException)
            {
                var failedRoleStorageException =
                    new FailedRoleStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedRoleStorageException);
            }
            catch (NotFoundRoleException notFoundRoleException)
            {
                throw CreateAndLogValidationException(notFoundRoleException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsRoleException =
                    new AlreadyExistsRoleException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsRoleException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidRoleReferenceException =
                    new InvalidRoleReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidRoleReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedRoleException = new LockedRoleException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedRoleException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedRoleStorageException =
                    new FailedRoleStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedRoleStorageException);
            }
            catch (Exception exception)
            {
                var failedRoleServiceException =
                    new FailedRoleServiceException(exception);

                throw CreateAndLogServiceException(failedRoleServiceException);
            }
        }

        private IQueryable<ApplicationRole> TryCatch(ReturningQueryableRoleFunction returningQueryableRoleFunction)
        {
            try
            {
                return returningQueryableRoleFunction();
            }
            catch (SqlException sqlException)
            {
                var failedRoleStorageException =
                    new FailedRoleStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedRoleStorageException);
            }
            catch (Exception exception)
            {
                var failedRoleServiceException =
                    new FailedRoleServiceException(exception);

                throw CreateAndLogServiceException(failedRoleServiceException);
            }
        }


        private RoleValidationException CreateAndLogValidationException(
             Xeption exception)
        {
            var RoleValidationException =
                new RoleValidationException(exception);

            this.loggingBroker.LogError(RoleValidationException);

            return RoleValidationException;
        }

        private RoleDependencyException CreateAndLogCriticalDependencyException(
            Xeption exception)
        {
            var RoleDependencyException = new RoleDependencyException(exception);
            this.loggingBroker.LogCritical(RoleDependencyException);

            return RoleDependencyException;
        }

        private RoleDependencyValidationException CreateAndLogDependencyValidationException(
        Xeption exception)
        {
            var RoleDependencyValidationException =
                new RoleDependencyValidationException(exception);

            this.loggingBroker.LogError(RoleDependencyValidationException);

            return RoleDependencyValidationException;
        }

        private RoleDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var RoleDependencyException = new RoleDependencyException(exception);
            this.loggingBroker.LogError(RoleDependencyException);

            return RoleDependencyException;
        }

        private RoleServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var RoleServiceException = new RoleServiceException(exception);
            this.loggingBroker.LogError(RoleServiceException);

            return RoleServiceException;
        }

        private RoleServiceException CreateAndLogServiceException(
            Exception exception)
        {
            var RoleServiceException = new RoleServiceException(exception);
            this.loggingBroker.LogError(RoleServiceException);

            return RoleServiceException;
        }
    }
}
