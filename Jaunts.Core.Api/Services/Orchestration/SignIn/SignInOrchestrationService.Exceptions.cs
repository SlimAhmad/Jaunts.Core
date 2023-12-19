using Jaunts.Core.Api.Models.Processings.SignIns.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Xeptions;

namespace Jaunts.Core.Api.Services.Orchestration.SignIn
{
    public partial class SignInOrchestrationService
    {
  
        private delegate ValueTask<bool> ReturningBooleanFunction();
        private delegate ValueTask ReturningFunction();
        private delegate ValueTask<ApplicationUser> ReturningUserFunction();

        private async ValueTask<bool> TryCatch(ReturningBooleanFunction returningBooleanFunction)
        {
            try
            {
                return await returningBooleanFunction();
            }
            catch (NullSignInProcessingException nullSignInProcessingException)
            {
                throw CreateAndLogValidationException(nullSignInProcessingException);
            }
            catch (InvalidSignInProcessingException invalidSignInProcessingException)
            {
                throw CreateAndLogValidationException(invalidSignInProcessingException);
            }
            catch (Exception exception)
            {
                var failedSignInProcessingServiceException =
                    new FailedSignInProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedSignInProcessingServiceException);
            }
        }

        private  ValueTask TryCatch(ReturningFunction returningFunction)
        {
            try
            {
                return  returningFunction();
            }
            catch (NullSignInProcessingException nullSignInProcessingException)
            {
                throw CreateAndLogValidationException(nullSignInProcessingException);
            }
            catch (InvalidSignInProcessingException invalidSignInProcessingException)
            {
                throw CreateAndLogValidationException(invalidSignInProcessingException);
            }
            catch (Exception exception)
            {
                var failedSignInProcessingServiceException =
                    new FailedSignInProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedSignInProcessingServiceException);
            }
        }

        private async ValueTask<ApplicationUser> TryCatch(ReturningUserFunction returningUserFunction)
        {
            try
            {
                return await returningUserFunction();
            }
            catch (NullSignInProcessingException nullSignInProcessingException)
            {
                throw CreateAndLogValidationException(nullSignInProcessingException);
            }
            catch (InvalidSignInProcessingException invalidSignInProcessingException)
            {
                throw CreateAndLogValidationException(invalidSignInProcessingException);
            }
            catch (Exception exception)
            {
                var failedSignInProcessingServiceException =
                    new FailedSignInProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedSignInProcessingServiceException);
            }
        }

        private SignInProcessingValidationException CreateAndLogValidationException(
             Xeption exception)
        {
            var SignInProcessingValidationException =
                new SignInProcessingValidationException(exception);

            this.loggingBroker.LogError(SignInProcessingValidationException);

            return SignInProcessingValidationException;
        }

        private SignInProcessingDependencyException CreateAndLogCriticalDependencyException(
            Xeption exception)
        {
            var SignInProcessingDependencyException = new SignInProcessingDependencyException(exception);
            this.loggingBroker.LogCritical(SignInProcessingDependencyException);

            return SignInProcessingDependencyException;
        }

        private SignInProcessingDependencyValidationException CreateAndLogDependencyValidationException(
        Xeption exception)
        {
            var SignInProcessingDependencyValidationException =
                new SignInProcessingDependencyValidationException(exception);

            this.loggingBroker.LogError(SignInProcessingDependencyValidationException);

            return SignInProcessingDependencyValidationException;
        }

        private SignInProcessingDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var SignInProcessingDependencyException = new SignInProcessingDependencyException(exception);
            this.loggingBroker.LogError(SignInProcessingDependencyException);

            return SignInProcessingDependencyException;
        }

        private SignInProcessingServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var SignInProcessingServiceException = new SignInProcessingServiceException(exception);
            this.loggingBroker.LogError(SignInProcessingServiceException);

            return SignInProcessingServiceException;
        }

        private SignInProcessingServiceException CreateAndLogServiceException(
            Exception exception)
        {
            var SignInProcessingServiceException = new SignInProcessingServiceException(exception);
            this.loggingBroker.LogError(SignInProcessingServiceException);

            return SignInProcessingServiceException;
        }
    }
}
