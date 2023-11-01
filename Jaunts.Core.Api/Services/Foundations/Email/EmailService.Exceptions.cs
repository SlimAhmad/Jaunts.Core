using Jaunts.Core.Models.Email;
using Jaunts.Core.Models.Exceptions;
using RESTFulSense.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.Email
{

    public partial class EmailService
    {
       

        private delegate ValueTask<SendEmailDetails> ReturningSendEmailDetailsFunction();


        private async ValueTask<SendEmailDetails> TryCatch(ReturningSendEmailDetailsFunction returningSendEmailDetailsFunction)
        {
            try
            {
                return await returningSendEmailDetailsFunction();
            }
            catch (NullEmailException nullEmailException)
            {
                throw new EmailValidationException(nullEmailException);
            }
            catch (InvalidEmailException invalidEmailException)
            {
                throw new EmailValidationException(invalidEmailException);
            }
            catch (HttpResponseUrlNotFoundException httpResponseUrlNotFoundException)
            {
                var invalidConfigurationEmailException =
                    new InvalidConfigurationEmailException(httpResponseUrlNotFoundException);

                throw new EmailDependencyException(invalidConfigurationEmailException);
            }
            catch (HttpResponseUnauthorizedException httpResponseUnauthorizedException)
            {
                var unauthorizedEmailException =
                    new UnauthorizedEmailException(httpResponseUnauthorizedException);

                throw new EmailDependencyException(unauthorizedEmailException);
            }
            catch (HttpResponseForbiddenException httpResponseForbiddenException)
            {
                var unauthorizedEmailException =
                    new UnauthorizedEmailException(httpResponseForbiddenException);

                throw new EmailDependencyException(unauthorizedEmailException);
            }
            catch (HttpResponseNotFoundException httpResponseNotFoundException)
            {
                var notFoundEmailException =
                    new NotFoundEmailException(httpResponseNotFoundException);

                throw new EmailDependencyValidationException(notFoundEmailException);
            }
            catch (HttpResponseBadRequestException httpResponseBadRequestException)
            {
                var invalidEmailException =
                    new InvalidEmailException(httpResponseBadRequestException);

                throw new EmailDependencyValidationException(invalidEmailException);
            }
            catch (HttpResponseTooManyRequestsException httpResponseTooManyRequestsException)
            {
                var excessiveCallEmailException =
                    new ExcessiveCallEmailException(httpResponseTooManyRequestsException);

                throw new EmailDependencyValidationException(excessiveCallEmailException);
            }
            catch (HttpResponseException httpResponseException)
            {
                var failedServerEmailException =
                    new FailedServerEmailException(httpResponseException);

                throw new EmailDependencyException(failedServerEmailException);
            }
            catch (Exception exception)
            {
                var failedEmailServiceException =
                    new FailedEmailServiceException(exception);

                throw new EmailServiceException(failedEmailServiceException);
            }
        }
  


    }
}