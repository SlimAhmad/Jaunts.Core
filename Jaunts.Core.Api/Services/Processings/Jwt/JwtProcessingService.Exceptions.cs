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
            catch (JwtValidationException jwtValidationException)
            {
                throw CreateAndLogDependencyValidationException(jwtValidationException);
            }
            catch (JwtDependencyValidationException jwtDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(jwtDependencyValidationException);
            }
            catch (JwtDependencyException jwtDependencyException)
            {
                throw CreateAndLogDependencyException(jwtDependencyException);
            }
            catch (JwtServiceException jwtServiceException)
            {
                throw CreateAndLogDependencyException(jwtServiceException);
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
            var jwtProcessingServiceException = new
                JwtProcessingServiceException(exception);

            this.loggingBroker.LogError(jwtProcessingServiceException);

            return jwtProcessingServiceException;
        }

        private JwtProcessingDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var jwtProcessingDependencyValidationException =
                new JwtProcessingDependencyValidationException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(jwtProcessingDependencyValidationException);

            return jwtProcessingDependencyValidationException;
        }

        private JwtProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var jwtProcessingDependencyException =
                new JwtProcessingDependencyException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(jwtProcessingDependencyException);

            return jwtProcessingDependencyException;
        }

        private JwtProcessingValidationException CreateAndLogValidationException(
            Xeption exception)
        {
            var jwtProcessingValidationException =
                new JwtProcessingValidationException(exception);

            this.loggingBroker.LogError(jwtProcessingValidationException);

            return jwtProcessingValidationException;
        }
    }
}