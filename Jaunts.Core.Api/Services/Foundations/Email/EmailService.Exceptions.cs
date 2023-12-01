using Jaunts.Core.Models.Email;
using Jaunts.Core.Models.Exceptions;
using RESTFulSense.Exceptions;
using Xeptions;

namespace Jaunts.Core.Api.Services.Foundations.Email
{

    public partial class EmailService
    {
       
        private delegate ValueTask<SendEmailResponse> ReturningSendEmailResponseFunction();
        private async ValueTask<SendEmailResponse> TryCatch(ReturningSendEmailResponseFunction returningSendEmailResponseFunction)
        {
            try
            {
                return await returningSendEmailResponseFunction();
            }
            catch (NullEmailException nullEmailException)
            {
                throw CreateAndLogValidationException(nullEmailException);
            }
            catch (InvalidEmailException invalidEmailException)
            {
                throw CreateAndLogValidationException(invalidEmailException);
            }
            catch (HttpResponseUrlNotFoundException httpResponseUrlNotFoundException)
            {
                var invalidConfigurationEmailException =
                    new InvalidConfigurationEmailException(httpResponseUrlNotFoundException);

                throw CreateAndLogDependencyException(invalidConfigurationEmailException);
            }
            catch (HttpResponseUnauthorizedException httpResponseUnauthorizedException)
            {
                var unauthorizedEmailException =
                    new UnauthorizedEmailException(httpResponseUnauthorizedException);

                throw CreateAndLogDependencyException(unauthorizedEmailException);
            }
            catch (HttpResponseForbiddenException httpResponseForbiddenException)
            {
                var unauthorizedEmailException =
                    new UnauthorizedEmailException(httpResponseForbiddenException);

                throw CreateAndLogDependencyException(unauthorizedEmailException);
            }
            catch (HttpResponseNotFoundException httpResponseNotFoundException)
            {
                var notFoundEmailException =
                    new NotFoundEmailException(httpResponseNotFoundException);

                throw CreateAndLogDependencyValidationException(notFoundEmailException);
            }
            catch (HttpResponseBadRequestException httpResponseBadRequestException)
            {
                var invalidEmailException =
                    new InvalidEmailException(httpResponseBadRequestException);

                throw CreateAndLogDependencyValidationException(invalidEmailException);
            }
            catch (HttpResponseTooManyRequestsException httpResponseTooManyRequestsException)
            {
                var excessiveCallEmailException =
                    new ExcessiveCallEmailException(httpResponseTooManyRequestsException);

                throw CreateAndLogDependencyValidationException(excessiveCallEmailException);
            }
            catch (HttpResponseException httpResponseException)
            {
                var failedServerEmailException =
                    new FailedServerEmailException(httpResponseException);

                throw CreateAndLogDependencyException(failedServerEmailException);
            }
            catch (Exception exception)
            {
                var failedEmailServiceException =
                    new FailedEmailServiceException(exception);

                throw CreateAndLogServiceException(failedEmailServiceException);
            }
        }

        private EmailValidationException CreateAndLogValidationException(
                    Xeption exception)
        {
            var EmailValidationException =
                new EmailValidationException(exception);

            this.loggingBroker.LogError(EmailValidationException);

            return EmailValidationException;
        }

        private EmailDependencyException CreateAndLogCriticalDependencyException(
            Xeption exception)
        {
            var EmailDependencyException = new EmailDependencyException(exception);
            this.loggingBroker.LogCritical(EmailDependencyException);

            return EmailDependencyException;
        }

        private EmailDependencyValidationException CreateAndLogDependencyValidationException(
        Xeption exception)
        {
            var EmailDependencyValidationException =
                new EmailDependencyValidationException(exception);

            this.loggingBroker.LogError(EmailDependencyValidationException);

            return EmailDependencyValidationException;
        }

        private EmailDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var EmailDependencyException = new EmailDependencyException(exception);
            this.loggingBroker.LogError(EmailDependencyException);

            return EmailDependencyException;
        }

        public EmailServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var EmailServiceException = new EmailServiceException(exception);
            this.loggingBroker.LogError(EmailServiceException);

            return EmailServiceException;
        }

        public EmailServiceException CreateAndLogServiceException(
            Exception exception)
        {
            var EmailServiceException = new EmailServiceException(exception);
            this.loggingBroker.LogError(EmailServiceException);

            return EmailServiceException;
        }

    }
}