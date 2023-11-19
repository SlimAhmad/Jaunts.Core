using Jaunts.Core.Api.Models.Jwt.Exceptions;
using Jaunts.Core.Api.Models.Processings.Jwts.Exceptions;
using Xeptions;

namespace Jaunts.Core.Api.Services.Processings.Jwt
{

    public partial class JwtProcessingService
    {
       

        private delegate ValueTask<string> ReturningStringFunction();
        private delegate ValueTask<bool> ReturningBooleanFunction();

        private async ValueTask<string> TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
            }
            catch (NullJwtProcessingException nullJwtProcessingException)
            {
                throw CreateAndLogValidationException(nullJwtProcessingException);
            }
            catch (InvalidJwtProcessingException invalidJwtProcessingException)
            {
                throw CreateAndLogValidationException(invalidJwtProcessingException);
            }
            catch (JwtValidationException countryValidationException)
            {
                throw CreateAndLogDependencyValidationException(countryValidationException);
            }
            catch (JwtDependencyValidationException countryDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(countryDependencyValidationException);
            }
            catch (JwtDependencyException countryDependencyException)
            {
                throw CreateAndLogDependencyException(countryDependencyException);
            }
            catch (JwtServiceException countryServiceException)
            {
                throw CreateAndLogDependencyException(countryServiceException);
            }
            catch (Exception exception)
            {
                var failedJwtProcessingServiceException =
                    new FailedJwtProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedJwtProcessingServiceException);
            }
        }

        private JwtProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var countryProcessingServiceException = new
                JwtProcessingServiceException(exception);

            this.loggingBroker.LogError(countryProcessingServiceException);

            return countryProcessingServiceException;
        }

        private JwtProcessingDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var countryProcessingDependencyValidationException =
                new JwtProcessingDependencyValidationException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(countryProcessingDependencyValidationException);

            return countryProcessingDependencyValidationException;
        }

        private JwtProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var countryProcessingDependencyException =
                new JwtProcessingDependencyException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(countryProcessingDependencyException);

            return countryProcessingDependencyException;
        }

        private JwtProcessingValidationException CreateAndLogValidationException(
            Xeption exception)
        {
            var countryProcessingValidationException =
                new JwtProcessingValidationException(exception);

            this.loggingBroker.LogError(countryProcessingValidationException);

            return countryProcessingValidationException;
        }
    }
}