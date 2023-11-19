using System.Text.RegularExpressions;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Auth.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.User.Exceptions;
using Jaunts.Core.Models.Auth.LoginRegister;
using Microsoft.AspNetCore.Identity;

namespace Jaunts.Core.Api.Services.Orchestration.Jwt
{
    public partial class JwtOrchestrationService
    {
        private void ValidateUserOnRegister(RegisterUserApiRequest apiRequest)
        {
            ValidateRegisterUserIsNull(apiRequest);

        }

        private void ValidateUserResponse(ApplicationUser user)
        {
            ValidateUserOnRegisterResponseIsNull(user);
        }

        public void ValidateSignIn(bool code) =>
           Validate((Rule: IsInvalidCode(code), Parameter: nameof(ApplicationUser)));


        private static void ValidateRegisterUserIsNull(RegisterUserApiRequest apiRequest)
        {
            if (apiRequest is null)
            {
                throw new NullUserException();
            }
        }

        private static void ValidateUserOnRegisterResponseIsNull(ApplicationUser user)
        {
            if (user is null)
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

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private dynamic IsNotValidEmail(string email) => new
        {
            Condition = !IsValidEmail(email),
            Message = "Invalid Email"
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

        private bool IsValidEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            return regex.IsMatch(email);
        }

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static void ValidateAgainstStorageUserOnModify(
            ApplicationUser inputUser, ApplicationUser storageUser)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputUser.CreatedDate,
                    secondDate: storageUser.CreatedDate,
                    secondDateName: nameof(ApplicationUser.CreatedDate)),
                Parameter: nameof(inputUser.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputUser.UpdatedDate,
                    secondDate: storageUser.UpdatedDate,
                    secondDateName: nameof(storageUser.UpdatedDate)),
                Parameter: nameof(storageUser.UpdatedDate)));
        }

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
