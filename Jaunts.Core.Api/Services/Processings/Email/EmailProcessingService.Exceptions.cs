using Jaunts.Core.Api.Models.Processings.Emails.Exceptions;
using Jaunts.Core.Api.Models.Processings.Role.Exceptions;
using Jaunts.Core.Api.Models.Role.Exceptions;
using Jaunts.Core.Models.Email;
using Xeptions;

namespace Jaunts.Core.Api.Services.Processings.Email
{

    public partial class EmailProcessingService
    {
       

        private delegate ValueTask<SendEmailResponse> ReturningSendEmailResponseFunction();


        private async ValueTask<SendEmailResponse> TryCatch(ReturningSendEmailResponseFunction returningSendEmailResponseFunction)
        {
            try
            {
                return await returningSendEmailResponseFunction();
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

        private EmailProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var countryProcessingServiceException = new
                EmailProcessingServiceException(exception);

            this.loggingBroker.LogError(countryProcessingServiceException);

            return countryProcessingServiceException;
        }

        private EmailProcessingDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var countryProcessingDependencyValidationException =
                new EmailProcessingDependencyValidationException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(countryProcessingDependencyValidationException);

            return countryProcessingDependencyValidationException;
        }

        private EmailProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var countryProcessingDependencyException =
                new EmailProcessingDependencyException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(countryProcessingDependencyException);

            return countryProcessingDependencyException;
        }

        private EmailProcessingValidationException CreateAndLogValidationException(
            Xeption exception)
        {
            var countryProcessingValidationException =
                new EmailProcessingValidationException(exception);

            this.loggingBroker.LogError(countryProcessingValidationException);

            return countryProcessingValidationException;
        }

    }
}