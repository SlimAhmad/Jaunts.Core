using Jaunts.Core.Models.Exceptions;
using Jaunts.Core.Models.Email;
using Jaunts.Core.Api.Models.Services.Foundations.Users;

namespace Jaunts.Core.Api.Services.Foundations.Email
{
    public partial class EmailService
    {
        private static void ValidateMail(SendEmailMessage sendEmailDetails)
        {
            ValidateMailIsNotNull(sendEmailDetails);

            Validate(
                (Rule: IsInvalid(sendEmailDetails.Subject), Parameter: nameof(SendEmailMessage.Subject)),
                (Rule: IsInvalid(sendEmailDetails.Html), Parameter: nameof(SendEmailMessage.Html)),
                (Rule: IsInvalid(sendEmailDetails.To), Parameter: nameof(SendEmailMessage.To)),
                (Rule: IsInvalid(sendEmailDetails.From), Parameter: nameof(SendEmailMessage.From)));
        }

        private static void ValidateMailResponse(SendEmailResponse  emailResponse)
        {
            if (!emailResponse.Successful)
            {
                foreach (var errors in emailResponse.Errors)
                {
                    Validate(
                        (Rule: ErrorsExist(errors), Parameter: nameof(SendEmailResponse)));
                }
            }
        }

        private static void ValidateMailIsNotNull(SendEmailMessage emailMessage)
        {
            if (emailMessage is null)
            {
               throw new NullEmailException();
            }
        }
        private static dynamic IsInvalid(object @object) => new
        {
            Condition = @object is null,
            Message = "Value is required"
        };
        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic ErrorsExist(string error) => new
        {
            Condition = !String.IsNullOrWhiteSpace(error),
            Message = error
        };
        private static dynamic IsInvalid(double number) => new
        {
            Condition = number >= 0,
            Message = "Value is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidEmailException = new InvalidEmailException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidEmailException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidEmailException.ThrowIfContainsErrors();
        }
    }
}