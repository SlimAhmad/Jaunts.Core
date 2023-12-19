using System.Text.RegularExpressions;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Auth.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.User.Exceptions;
using Jaunts.Core.Models.Auth.LoginRegister;
using Microsoft.AspNetCore.Identity;

namespace Jaunts.Core.Api.Services.Aggregations.Account
{
    public partial class AccountAggregationService
    {
        private void ValidateUserOnRegister(RegisterUserApiRequest apiRequest)
        {
            ValidateRegisterUserIsNull(apiRequest);

            Validate(
                (Rule: IsInvalid(apiRequest.FirstName), Parameter: nameof(RegisterUserApiRequest.FirstName)),
                (Rule: IsInvalid(apiRequest.LastName), Parameter: nameof(RegisterUserApiRequest.LastName)),
                (Rule: IsInvalid(apiRequest.PhoneNumber), Parameter: nameof(RegisterUserApiRequest.PhoneNumber)),
                (Rule: IsInvalid(apiRequest.Username), Parameter: nameof(RegisterUserApiRequest.Username)),
                (Rule: IsInvalid(apiRequest.Email), Parameter: nameof(RegisterUserApiRequest.Email)));

      
         
        }

        private void ValidateResetPassword(ResetPasswordApiRequest resetPassword)
        {
            ValidateResetPasswordIsNull(resetPassword);

            Validate(
               (Rule: IsInvalid(resetPassword.Password), Parameter: nameof(ResetPasswordApiRequest.Password)),
               (Rule: IsInvalid(resetPassword.ConfirmPassword), Parameter: nameof(ResetPasswordApiRequest.ConfirmPassword)),
               (Rule: IsInvalid(resetPassword.Token), Parameter: nameof(ResetPasswordApiRequest.Token)));

               
        }

        private void ValidateUserResponse(ApplicationUser user)
        {
            ValidateUserOnRegisterResponseIsNull(user);
            ValidateUserOnRegisterRequestIsNull(user);

            Validate(
               (Rule: IsInvalid(user.FirstName), Parameter: nameof(ApplicationUser.FirstName)),
               (Rule: IsInvalid(user.LastName), Parameter: nameof(ApplicationUser.LastName)),
               (Rule: IsInvalid(user.UserName), Parameter: nameof(ApplicationUser.UserName)),
               (Rule: IsInvalid(user.PhoneNumber), Parameter: nameof(ApplicationUser.PhoneNumber)));


        }

        private void ValidateUserOnRegisterRequestIsNull(ApplicationUser user)
        {
            Validate(
                 (Rule: IsInvalidUser(user), Parameter: nameof(ApplicationUser)));
        }

        private void ValidateIdentityResultResponse(IdentityResult identityResult)
        {
            if (!identityResult.Succeeded)
            {
                foreach (var errors in identityResult.Errors)
                {
                    Validate(
                         (Rule: IsInvalid(errors), Parameter: nameof(IdentityError)));
                }
            }
        }

        private void ValidateUserOnLogin(LoginRequest request)
        {
            ValidateUserOnLoginIsNull(request);

            Validate(
               (Rule: IsInvalid(request.Password), Parameter: nameof(ApplicationUser.PasswordHash)),
               (Rule: IsInvalid(request.UsernameOrEmail), Parameter: nameof(ApplicationUser.UserName)),
               (Rule: IsInvalid(request.UsernameOrEmail), Parameter: nameof(ApplicationUser.Email)));
        }

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

        private static void ValidateStorageUser(ApplicationUser storageUser, Guid userId)
        {
            if (storageUser is null)
            {
                throw new NotFoundUserException(userId);
            }
        }

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

        private static void ValidateResetPasswordIsNull(ResetPasswordApiRequest resetPassword)
        {
            if (resetPassword is null)
            {
                throw new NullUserException();
            }
        }

        private static void ValidateUserOnLoginIsNull(LoginRequest request)
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

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
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
