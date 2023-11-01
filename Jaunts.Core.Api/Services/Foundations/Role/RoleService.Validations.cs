using Jaunts.Core.Api.Models.Role.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Models.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.Role
{
    public partial class RoleService
    {
        private void ValidateRoleOnCreate(ApplicationRole role)
        {
            ValidateRoleIsNull(role);

            Validate(
                (Rule: IsInvalid(role.Name), Parameter: nameof(ApplicationRole.Name)),

                (Rule: IsNotSame(
                    firstDate: role.UpdatedDate,
                    secondDate: role.CreatedDate,
                    secondDateName: nameof(role.CreatedDate)),
                Parameter: nameof(role.UpdatedDate)),

                (Rule: IsNotRecent(role.UpdatedDate), Parameter: nameof(role.UpdatedDate)));


        }

        private void ValidateRoleOnModify(ApplicationRole role)
        {
            ValidateRoleIsNull(role);

            Validate(
               (Rule: IsInvalid(role.Id), Parameter: nameof(ApplicationRole.Id)),
               (Rule: IsInvalid(role.Name), Parameter: nameof(ApplicationRole.Name)),

               (Rule: IsSame(
                    firstDate: role.UpdatedDate,
                    secondDate: role.CreatedDate,
                    secondDateName: nameof(role.CreatedDate)),
                Parameter: nameof(role.UpdatedDate)),

                (Rule: IsNotRecent(role.UpdatedDate), Parameter: nameof(role.UpdatedDate)));


        }

        public void ValidateRoleId(Guid roleId) =>
             Validate((Rule: IsInvalid(roleId), Parameter: nameof(ApplicationRole.Id)));

        public void ValidateRole(string role) =>
             Validate((Rule: IsInvalid(role), Parameter: nameof(ApplicationRole.Name)));

        private static void ValidateStorageRole(ApplicationRole storageRole, Guid roleId)
        {
            if (storageRole is null)
            {
                throw new NotFoundRoleException(roleId);
            }
        }


        private static void ValidateRoleIsNull(ApplicationRole role)
        {
            if (role is null)
            {
                throw new NullRoleException();
            }
        }

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

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTime();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static void ValidateAgainstStorageRoleOnModify(ApplicationRole inputRole, ApplicationRole storageRole)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputRole.CreatedDate,
                    secondDate: storageRole.CreatedDate,
                    secondDateName: nameof(ApplicationRole.CreatedDate)),
                Parameter: nameof(inputRole.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputRole.UpdatedDate,
                    secondDate: storageRole.UpdatedDate,
                    secondDateName: nameof(storageRole.UpdatedDate)),
                Parameter: nameof(storageRole.UpdatedDate)));
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidresetPasswordException = new InvalidEmailException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidresetPasswordException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidresetPasswordException.ThrowIfContainsErrors();
        }
    }
}
