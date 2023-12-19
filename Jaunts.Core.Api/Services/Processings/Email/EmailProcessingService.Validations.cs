using Jaunts.Core.Models.Exceptions;
using Jaunts.Core.Models.Email;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.Processings.Emails.Exceptions;

namespace Jaunts.Core.Api.Services.Processings.Email
{
    public partial class EmailProcessingService
    {
        

        private void ValidateUser(ApplicationUser user)
        {
            if (user is null)
            {
                throw new NullEmailProcessingException();
            }
        }

        private void ValidateEmailResponse(SendEmailMessage emailMessage)
        {
            if (emailMessage is null)
            {
                throw new NullEmailProcessingException();
            }
        }
        public void ValidateToken(string text)=>
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
            var invalidEmailProcessingException = new InvalidEmailProcessingException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidEmailProcessingException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidEmailProcessingException.ThrowIfContainsErrors();
        }
    }
}