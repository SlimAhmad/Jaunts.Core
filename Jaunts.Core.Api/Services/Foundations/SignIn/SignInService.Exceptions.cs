using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.SignIn.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace Jaunts.Core.Api.Services.Foundations.SignIn
{
    public partial class SignInService
    {
        private delegate ValueTask<SignInResult> ReturningSignInFunction();

        private async ValueTask<SignInResult> TryCatch(ReturningSignInFunction returningSignInFunction)
        {
            try
            {
                return await returningSignInFunction();
            }
            catch (NullSignInException nullSignInException)
            {
                throw CreateAndLogValidationException(nullSignInException);
            }
            catch (InvalidSignInException invalidSignInException)
            {
                throw CreateAndLogValidationException(invalidSignInException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedSignInStorageException =
                    new FailedSignInStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedSignInStorageException);
            }
            catch (Exception exception)
            {
                var failedSignInServiceException =
                    new FailedSignInServiceException(exception);

                throw CreateAndLogServiceException(failedSignInServiceException);
            }
        }

        private SignInValidationException CreateAndLogValidationException(
             Xeption exception)
        {
            var SignInValidationException =
                new SignInValidationException(exception);

            this.loggingBroker.LogError(SignInValidationException);

            return SignInValidationException;
        }

        private SignInDependencyException CreateAndLogCriticalDependencyException(
            Xeption exception)
        {
            var SignInDependencyException = new SignInDependencyException(exception);
            this.loggingBroker.LogCritical(SignInDependencyException);

            return SignInDependencyException;
        }

        private SignInDependencyValidationException CreateAndLogDependencyValidationException(
        Xeption exception)
        {
            var SignInDependencyValidationException =
                new SignInDependencyValidationException(exception);

            this.loggingBroker.LogError(SignInDependencyValidationException);

            return SignInDependencyValidationException;
        }

        private SignInDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var SignInDependencyException = new SignInDependencyException(exception);
            this.loggingBroker.LogError(SignInDependencyException);

            return SignInDependencyException;
        }

        private SignInServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var SignInServiceException = new SignInServiceException(exception);
            this.loggingBroker.LogError(SignInServiceException);

            return SignInServiceException;
        }

        private SignInServiceException CreateAndLogServiceException(
            Exception exception)
        {
            var SignInServiceException = new SignInServiceException(exception);
            this.loggingBroker.LogError(SignInServiceException);

            return SignInServiceException;
        }
    }
}
