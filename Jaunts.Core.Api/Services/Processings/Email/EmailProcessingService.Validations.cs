using Jaunts.Core.Models.Exceptions;
using Jaunts.Core.Models.Email;
using Jaunts.Core.Api.Models.Services.Foundations.Users;

namespace Jaunts.Core.Api.Services.Processings.Email
{
    public partial class EmailProcessingService
    {
        private static void ValidateMail(SendEmailDetails sendEmailDetails)
        {
            ValidateMailNotNull(sendEmailDetails);
        }

        private void ValidateUser(ApplicationUser user)
        {
            ValidateUserNotNull(user);
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