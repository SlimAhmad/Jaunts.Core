using Jaunts.Core.Models.Exceptions;
using Jaunts.Core.Models.Email;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using System.Diagnostics.Metrics;
using Jaunts.Core.Api.Models.Processings.User.Exceptions;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.User.Exceptions;
using Jaunts.Core.Models.Auth.LoginRegister;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Dna;

namespace Jaunts.Core.Api.Services.Processings.User
{
    public partial class UserProcessingService
    {
        private static void ValidateUser(ApplicationUser user)
        {
            ValidateUserIsNotNull(user);

            Validate(
            (Rule: IsInvalid(user.Id),
                Parameter: nameof(ApplicationUser.Id)));
        }

        private static void ValidateUserLoginIsNotNull(LoginCredentialsRequest request)
        {
            if (request is null)
            {
                throw new NullUserProcessingException();
            }
        }

        private static void ValidateUserResponseIsNotNull(ApplicationUser user)
        {
            if (user is null)
            {
                throw new NullUserProcessingException();
            }
        }

        private static void ValidateResetPasswordIsNull(ResetPasswordApiRequest  request)
        {
            if (request is null)
            {
                throw new NullUserProcessingException();
            }
        }

        private static void ValidateUserIsNotNull(ApplicationUser user)
        {
            if (user is null)
            {
                throw new NullUserProcessingException();
            }
        }

        public void ValidateUserId(Guid userId) =>
           Validate((Rule: IsInvalid(userId), Parameter: nameof(ApplicationUser.Id)));
        public void ValidateStringIsNotNull(string text) =>
           Validate((Rule: IsInvalid(text), Parameter: nameof(ApplicationUser)));

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrEmpty(text),
            Message = "Value is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidUserProcessingException =
                new InvalidUserProcessingException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidUserProcessingException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidUserProcessingException.ThrowIfContainsErrors();
        }
    }
}