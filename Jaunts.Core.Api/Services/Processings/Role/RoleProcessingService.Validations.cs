using Jaunts.Core.Api.Models.Role.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;

namespace Jaunts.Core.Api.Services.Processings.Role
{
    public partial class RoleProcessingService
    {
        private static void ValidateRole(ApplicationRole  applicationRole)
        {

            if (applicationRole is null)
            {
                throw new NullRoleException();
            }

        }



        private static void ValidateUserNotNull(ApplicationUser user)
        {
            if (user is null)
            {
                throw new NullRoleException();
            }
        }

        public void ValidateRoleList(IList<string> roles) =>
           Validate((Rule: IsInvalid(roles), Parameter: nameof(ApplicationRole)));

        private static dynamic IsInvalid(object @object) => new
        {
            Condition = @object is null,
            Message = "Value is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidRoleException = new InvalidRoleException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidRoleException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidRoleException.ThrowIfContainsErrors();
        }
    }
}