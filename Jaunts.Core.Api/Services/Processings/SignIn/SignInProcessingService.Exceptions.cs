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
            catch (SignInValidationException countryValidationException)
            {
                throw CreateAndLogDependencyValidationException(countryValidationException);
            }
            catch (SignInDependencyValidationException countryDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(countryDependencyValidationException);
            }
            catch (SignInDependencyException countryDependencyException)
            {
                throw CreateAndLogDependencyException(countryDependencyException);
            }
            catch (SignInServiceException countryServiceException)
            {
                throw CreateAndLogDependencyException(countryServiceException);
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
            catch (SignInValidationException countryValidationException)
            {
                throw CreateAndLogDependencyValidationException(countryValidationException);
            }
            catch (SignInDependencyValidationException countryDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(countryDependencyValidationException);
            }
            catch (SignInDependencyException countryDependencyException)
            {
                throw CreateAndLogDependencyException(countryDependencyException);
            }
            catch (SignInServiceException countryServiceException)
            {
                throw CreateAndLogDependencyException(countryServiceException);
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
            var countryProcessingServiceException = new
                SignInProcessingServiceException(exception);

            this.loggingBroker.LogError(countryProcessingServiceException);

            return countryProcessingServiceException;
        }

        private SignInProcessingDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var countryProcessingDependencyValidationException =
                new SignInProcessingDependencyValidationException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(countryProcessingDependencyValidationException);

            return countryProcessingDependencyValidationException;
        }

        private SignInProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var countryProcessingDependencyException =
                new SignInProcessingDependencyException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(countryProcessingDependencyException);

            return countryProcessingDependencyException;
        }

        private SignInProcessingValidationException CreateAndLogValidationException(
            Xeption exception)
        {
            var countryProcessingValidationException =
                new SignInProcessingValidationException(exception);

            this.loggingBroker.LogError(countryProcessingValidationException);

            return countryProcessingValidationException;
        }
    }
}