using System.Text.RegularExpressions;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Auth.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.User.Exceptions;
using Jaunts.Core.Models.Auth.LoginRegister;
using Microsoft.AspNetCore.Identity;

namespace Jaunts.Core.Api.Services.Orchestration.Email
{
    public partial class EmailOrchestrationService
    {

        public void ValidateUserId(Guid userId) =>
             Validate((Rule: IsInvalid(userId), Parameter: nameof(ApplicationUser.Id)));

        public void ValidateUserEmail(string text) =>
             Validate((Rule: IsInvalid(text), Parameter: nameof(ForgotPasswordApiResponse)));

        public void ValidateUserProfileDetails(string text) =>
             Validate((Rule: IsInvalid(text), Parameter: nameof(UserAccountDetailsResponse)));

        public void ValidateUserPassword(bool password) =>
            Validate((Rule: IsNotValidPassword(password), Parameter: nameof(ApplicationUser)));

        public void ValidateSignIn(bool code) =>
           Validate((Rule: IsInvalidCode(code), Parameter: nameof(ApplicationUser)));


        private static void ValidateUserOnLoginIsNull(LoginCredentialsRequest request)
        {
            if (request is null)
            {
                throw new NullUserException();
            }
        }

        private static dynamic IsInvalid(object @object) => new
        {
            Condition = @object is null,
            Message = "Value is required"
        };

        private static dynamic IsInvalidUser(object @object) => new
        {
            Condition = @object is null,
            Message = "User not found"
        };

        private static dynamic IsInvalidResult(IdentityError @object) => new
        {
            Condition = @object.Code != null,
            Message = @object.Description
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(IdentityError error) => new
        {
            Condition = !String.IsNullOrWhiteSpace(error.Code),
            Message = error.Description
        };

        private static dynamic IsInvalid(double number) => new
        {
            Condition = number >= 0,
            Message = "Number is required"
        };

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private dynamic IsNotValidPassword(bool password) => new
        {
            Condition = password is false,
            Message = "Invalid password or email"
        };

        private dynamic IsInvalidCode(bool code) => new
        {
            Condition = code is false,
            Message = "Invalid OTP code"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidAuthException = new InvalidAuthException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidAuthException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidAuthException.ThrowIfContainsErrors();
        }
    }
}
