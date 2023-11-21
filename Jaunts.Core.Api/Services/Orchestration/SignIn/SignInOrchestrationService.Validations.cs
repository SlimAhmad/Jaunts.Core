using System.Text.RegularExpressions;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Processings.SignIns.Exceptions;
using Jaunts.Core.Api.Models.Processings.User.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Auth.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.User.Exceptions;
using Jaunts.Core.Models.Auth.LoginRegister;
using Microsoft.AspNetCore.Identity;

namespace Jaunts.Core.Api.Services.Orchestration.SignIn
{
    public partial class SignInOrchestrationService
    {
        public void ValidateUser(ApplicationUser user)
        {
            if (user is null)
            {
                throw new NullUserProcessingException();
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
