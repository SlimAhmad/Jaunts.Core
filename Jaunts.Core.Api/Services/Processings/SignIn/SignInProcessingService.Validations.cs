using Jaunts.Core.Api.Models.SignIn.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Models.Processings.User.Exceptions;
using Jaunts.Core.Api.Models.Processings.SignIns.Exceptions;

namespace Jaunts.Core.Api.Services.Processings.SignIn
{
    public partial class SignInProcessingService
    {

        public void ValidateUser(ApplicationUser user)
        {
            if (user == null)
            {
                throw new NullSignInProcessingException();
            }
        }
        public void ValidateString(string text) =>
               Validate((Rule: IsInvalid(text), Parameter: nameof(ApplicationUser)));
        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidSignInProcessingException = new InvalidSignInProcessingException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidSignInProcessingException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidSignInProcessingException.ThrowIfContainsErrors();
        }
    }
}