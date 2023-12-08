using Jaunts.Core.Api.Models.Processings.Emails.Exceptions;
using Jaunts.Core.Models.Email;
using Jaunts.Core.Models.Exceptions;
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
            catch (NullEmailProcessingException nullEmailProcessingException)
            {
                throw CreateAndLogValidationException(nullEmailProcessingException);
            }
            catch (InvalidEmailException invalidEmailProcessingException)
            {
                throw CreateAndLogValidationException(invalidEmailProcessingException);
            }
            catch (EmailValidationException roleValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleValidationException);
            }
            catch (EmailDependencyValidationException roleDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleDependencyValidationException);
            }
            catch (EmailDependencyException roleDependencyException)
            {
                throw CreateAndLogDependencyException(roleDependencyException);
            }
            catch (EmailServiceException roleServiceException)
            {
                throw CreateAndLogDependencyException(roleServiceException);
            }
            catch (Exception exception)
            {
                var failedEmailProcessingServiceException =
                    new FailedEmailProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedEmailProcessingServiceException);
            }
        }

        private EmailProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var EmailProcessingServiceException = new
                EmailProcessingServiceException(exception);

            this.loggingBroker.LogError(EmailProcessingServiceException);

            return EmailProcessingServiceException;
        }

        private EmailProcessingDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var EmailProcessingDependencyValidationException =
                new EmailProcessingDependencyValidationException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(EmailProcessingDependencyValidationException);

            return EmailProcessingDependencyValidationException;
        }

        private EmailProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var EmailProcessingDependencyException =
                new EmailProcessingDependencyException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(EmailProcessingDependencyException);

            return EmailProcessingDependencyException;
        }

        private EmailProcessingValidationException CreateAndLogValidationException(
            Xeption exception)
        {
            var EmailProcessingValidationException =
                new EmailProcessingValidationException(exception);

            this.loggingBroker.LogError(EmailProcessingValidationException);

            return EmailProcessingValidationException;
        }

    }
}