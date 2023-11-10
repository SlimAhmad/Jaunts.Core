using Jaunts.Core.Models.Exceptions;
using Jaunts.Core.Models.Email;
using Jaunts.Core.Api.Models.Services.Foundations.Users;

namespace Jaunts.Core.Api.Services.Foundations.Email
{
    public partial class EmailService
    {
        private static void ValidateMail(SendEmailDetails sendEmailDetails)
        {
           
  
            Validate(
                (Rule: IsInvalid(sendEmailDetails.Subject), Parameter: nameof(SendEmailDetails.Subject)),
                (Rule: IsInvalid(sendEmailDetails.Text), Parameter: nameof(SendEmailDetails.Text)),
                (Rule: IsInvalid(sendEmailDetails.To), Parameter: nameof(SendEmailDetails.To)),
                (Rule: IsInvalid(sendEmailDetails.From), Parameter: nameof(SendEmailDetails.From))

                );

        }

        private void ValidateUser(ApplicationUser user)
        {
            ValidateUserNotNull(user);

            Validate(
               (Rule: IsInvalid(user.FirstName), Parameter: nameof(ApplicationUser.FirstName)),
               (Rule: IsInvalid(user.LastName), Parameter: nameof(ApplicationUser.LastName)),
               (Rule: IsInvalid(user.Email), Parameter: nameof(ApplicationUser.Email)));

             


        }
        private static void ValidateMailNotNull(SendEmailDetails sendEmailDetails)
        {
            if (sendEmailDetails is null)
            {
                throw new NullEmailException();
            }
        }


        private static void ValidateUserNotNull(ApplicationUser user)
        {
            if (user is null)
            {
                throw new NullEmailException();
            }
        }
      


        public void ValidateSendMail(string text)=>
            Validate((Rule: IsInvalid(text), Parameter: nameof(SendEmailResponse)));
        


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