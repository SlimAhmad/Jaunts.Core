using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.SignIn.Exceptions;
using Jaunts.Core.Api.Models.User.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Jaunts.Core.Api.Services.Foundations.SignIn
{
    public partial class SignInService
    {
        private void ValidateUser(ApplicationUser user)
        {
            ValidateUserIsNull(user);

            Validate(
               (Rule: IsInvalid(user.FirstName), Parameter: nameof(ApplicationUser.FirstName)),
               (Rule: IsInvalid(user.LastName), Parameter: nameof(ApplicationUser.LastName)),
               (Rule: IsInvalid(user.UserName), Parameter: nameof(ApplicationUser.UserName)),
               (Rule: IsInvalid(user.PhoneNumber), Parameter: nameof(ApplicationUser.PhoneNumber)),
               (Rule: IsInvalid(user.Email), Parameter: nameof(ApplicationUser.Email)),
               (Rule: IsInvalid(user.CreatedDate), Parameter: nameof(ApplicationUser.CreatedDate)),
               (Rule: IsInvalid(user.UpdatedDate), Parameter: nameof(ApplicationUser.UpdatedDate)));




        }

        private static void ValidateUserIsNull(ApplicationUser user)
        {
            if (user is null)
            {
                throw new NullUserException();
            }
        }

        public void ValidateText(string text) =>
             Validate((Rule: IsInvalid(text), Parameter: nameof(SignInResult)));

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

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidSignInException = new InvalidSignInException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidSignInException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidSignInException.ThrowIfContainsErrors();
        }
    }
}
