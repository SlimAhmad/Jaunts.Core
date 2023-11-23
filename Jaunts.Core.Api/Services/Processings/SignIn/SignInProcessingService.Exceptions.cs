using Jaunts.Core.Api.Models.SignIn.Exceptions;
using Jaunts.Core.Api.Models.Processings.SignIns.Exceptions;
using Xeptions;

namespace Jaunts.Core.Api.Services.Processings.SignIn
{

    public partial class SignInProcessingService
    {
       

        private delegate ValueTask ReturningFunction();
        private delegate ValueTask<bool> ReturningBooleanFunction();

        private ValueTask TryCatch(ReturningFunction returningFunction)
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
            catch (SignInValidationException signInValidationException)
            {
                throw CreateAndLogDependencyValidationException(signInValidationException);
            }
            catch (SignInDependencyValidationException signInDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(signInDependencyValidationException);
            }
            catch (SignInDependencyException signInDependencyException)
            {
                throw CreateAndLogDependencyException(signInDependencyException);
            }
            catch (SignInServiceException signInServiceException)
            {
                throw CreateAndLogDependencyException(signInServiceException);
            }
            catch (Exception exception)
            {
                var failedSignInProcessingServiceException =
                    new FailedSignInProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedSignInProcessingServiceException);
            }
        }

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
            catch (SignInValidationException signInValidationException)
            {
                throw CreateAndLogDependencyValidationException(signInValidationException);
            }
            catch (SignInDependencyValidationException signInDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(signInDependencyValidationException);
            }
            catch (SignInDependencyException signInDependencyException)
            {
                throw CreateAndLogDependencyException(signInDependencyException);
            }
            catch (SignInServiceException signInServiceException)
            {
                throw CreateAndLogDependencyException(signInServiceException);
            }
            catch (Exception exception)
            {
                var failedSignInProcessingServiceException =
                    new FailedSignInProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedSignInProcessingServiceException);
            }
        }
        private SignInProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var signInProcessingServiceException = new
                SignInProcessingServiceException(exception);

            this.loggingBroker.LogError(signInProcessingServiceException);

            return signInProcessingServiceException;
        }

        private SignInProcessingDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var signInProcessingDependencyValidationException =
                new SignInProcessingDependencyValidationException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(signInProcessingDependencyValidationException);

            return signInProcessingDependencyValidationException;
        }

        private SignInProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var signInProcessingDependencyException =
                new SignInProcessingDependencyException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(signInProcessingDependencyException);

            return signInProcessingDependencyException;
        }

        private SignInProcessingValidationException CreateAndLogValidationException(
            Xeption exception)
        {
            var signInProcessingValidationException =
                new SignInProcessingValidationException(exception);

            this.loggingBroker.LogError(signInProcessingValidationException);

            return signInProcessingValidationException;
        }
    }
}