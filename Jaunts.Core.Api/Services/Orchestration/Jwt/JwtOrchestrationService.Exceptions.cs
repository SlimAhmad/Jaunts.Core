using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Jwt.Exceptions;
using Xeptions;

namespace Jaunts.Core.Api.Services.Orchestration.Jwt
{
    public partial class JwtOrchestrationService
    {
  
        private delegate ValueTask<UserAccountDetailsApiResponse> ReturningUserAccountDetailsFunction();

        private async ValueTask<UserAccountDetailsApiResponse> TryCatch(ReturningUserAccountDetailsFunction returningUserAccountDetailsFunction)
        {
            try
            {
                return await returningUserAccountDetailsFunction();
            }
            catch (NullJwtException nullJwtException)
            {
                throw CreateAndLogValidationException(nullJwtException);
            }
            catch (InvalidJwtException invalidJwtException)
            {
                throw CreateAndLogValidationException(invalidJwtException);
            }
            catch (Exception exception)
            {
                var failedJwtServiceException =
                    new FailedJwtServiceException(exception);

                throw CreateAndLogServiceException(failedJwtServiceException);
            }
        }
     

        private JwtValidationException CreateAndLogValidationException(
             Xeption exception)
        {
            var JwtValidationException =
                new JwtValidationException(exception);

            this.loggingBroker.LogError(JwtValidationException);

            return JwtValidationException;
        }

        private JwtDependencyException CreateAndLogCriticalDependencyException(
            Xeption exception)
        {
            var JwtDependencyException = new JwtDependencyException(exception);
            this.loggingBroker.LogCritical(JwtDependencyException);

            return JwtDependencyException;
        }

        private JwtDependencyValidationException CreateAndLogDependencyValidationException(
        Xeption exception)
        {
            var JwtDependencyValidationException =
                new JwtDependencyValidationException(exception);

            this.loggingBroker.LogError(JwtDependencyValidationException);

            return JwtDependencyValidationException;
        }

        private JwtDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var JwtDependencyException = new JwtDependencyException(exception);
            this.loggingBroker.LogError(JwtDependencyException);

            return JwtDependencyException;
        }

        private JwtServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var JwtServiceException = new JwtServiceException(exception);
            this.loggingBroker.LogError(JwtServiceException);

            return JwtServiceException;
        }

        private JwtServiceException CreateAndLogServiceException(
            Exception exception)
        {
            var JwtServiceException = new JwtServiceException(exception);
            this.loggingBroker.LogError(JwtServiceException);

            return JwtServiceException;
        }
    }
}
