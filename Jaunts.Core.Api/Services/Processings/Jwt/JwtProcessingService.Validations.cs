using Jaunts.Core.Api.Models.Jwt.Exceptions;
using Jaunts.Core.Api.Models.Processings.Jwts.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;

namespace Jaunts.Core.Api.Services.Processings.Jwt
{
    public partial class JwtProcessingService
    {

        private static void ValidateUserNotNull(ApplicationUser user)
        {
            if (user is null)
            {
                throw new NullJwtProcessingException();
            }
        }

        private static dynamic IsInvalid(object @object) => new
        {
            Condition = @object is null,
            Message = "Value is required"
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