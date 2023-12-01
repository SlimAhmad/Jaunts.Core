using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.Jwt.Exceptions;
using Jaunts.Core.Api.Models.User.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Jaunts.Core.Api.Services.Foundations.Jwt
{
    public partial class JwtService
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
                throw new NullJwtException();
            }
        }

        public void ValidateText(double number) =>
             Validate((Rule: IsInvalid(number), Parameter: nameof(ApplicationUser)));

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
            Condition = number <= 0,
            Message = "Number is required"
        };


        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidJwtException = new InvalidJwtException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidJwtException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidJwtException.ThrowIfContainsErrors();
        }
    }
}
