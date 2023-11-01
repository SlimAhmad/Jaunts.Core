using Jaunts.Core.Models.Exceptions;
using Jaunts.Core.Models.Email;

namespace Jaunts.Core.Api.Services.Foundations.Email
{
    public partial class EmailService
    {
        private static void ValidateMail(SendEmailDetails sendEmailDetails)
        {
            ValidateMailNotNull(sendEmailDetails);
            ValidateMailRequest(sendEmailDetails);
          

            Validate(
                (Rule: IsInvalid(sendEmailDetails.Subject), Parameter: nameof(SendEmailDetails.Subject)),
                (Rule: IsInvalid(sendEmailDetails.Text), Parameter: nameof(SendEmailDetails.Text)),
                (Rule: IsInvalid(sendEmailDetails.To), Parameter: nameof(SendEmailDetails.To)),
                (Rule: IsInvalid(sendEmailDetails.From), Parameter: nameof(SendEmailDetails.From))

                );

        }


        private static void ValidateMailNotNull(SendEmailDetails sendEmailDetails)
        {
            if (sendEmailDetails is null)
            {
                throw new NullEmailException();
            }
        }

        private static void ValidateMailRequest(SendEmailDetails sendEmailDetailsRequest)
        {
            Validate((Rule: IsInvalid(sendEmailDetailsRequest), Parameter: nameof(SendEmailDetails)));
        }

 



        private static dynamic IsInvalid(object @object) => new
        {
            Condition = @object is null,
            Message = "Value is required"
        };


        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Value is required"
        };

        private static dynamic IsInvalid(double number) => new
        {
            Condition = number >= 0,
            Message = "Value is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidresetPasswordException = new InvalidEmailException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidresetPasswordException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidresetPasswordException.ThrowIfContainsErrors();
        }
    }
}