using Jaunts.Core.Api.Models.Processings.Role.Exceptions;
using Jaunts.Core.Api.Models.Role.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;
using System.Diagnostics.Metrics;

namespace Jaunts.Core.Api.Services.Processings.Role
{
    public partial class RoleProcessingService
    {
        private static void ValidateRole(ApplicationRole role)
        {
            ValidateRoleIsNotNull(role);

            Validate(
            (Rule: IsInvalid(role.Id),
                Parameter: nameof(ApplicationRole.Id)));
        }
        private static void ValidateRoleIsNotNull(ApplicationRole  applicationRole)
        {
            if (applicationRole is null)
            {
                throw new NullRoleProcessingException();
            }
        }

        private static void ValidateUserNotNull(ApplicationUser user)
        {
            if (user is null)
            {
                throw new NullRoleProcessingException();
            }
        }

        public void ValidateRoleList(IList<string> roles) =>
           Validate((Rule: IsInvalid(roles), Parameter: nameof(ApplicationRole)));
        public void ValidateRoleId(Guid roleId) =>
           Validate((Rule: IsInvalid(roleId), Parameter: nameof(ApplicationRole.Id)));

        private static dynamic IsInvalid(object @object) => new
        {
            Condition = @object is null,
            Message = "Value is required"
        };

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };
        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidRoleProcessingException = new InvalidRoleProcessingException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidRoleProcessingException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidRoleProcessingException.ThrowIfContainsErrors();
        }
    }
}