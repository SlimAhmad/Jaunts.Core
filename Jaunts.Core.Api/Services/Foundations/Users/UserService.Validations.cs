﻿using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.Services.Foundations.Users.Exceptions;
using Jaunts.Core.Api.Models.User.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Jaunts.Core.Api.Services.Foundations.Users
{
    public partial class UserService
    {

      

        private void ValidateUserOnAdd(ApplicationUser user, string password)
        {
            ValidateUserIsNull(user);
            Validate(
                (Rule: IsInvalid(user.FirstName), Parameter: nameof(ApplicationUser.FirstName)),
                (Rule: IsInvalid(user.LastName), Parameter: nameof(ApplicationUser.LastName)),
                (Rule: IsInvalid(user.UserName), Parameter: nameof(ApplicationUser.UserName)),
                (Rule: IsInvalid(user.PhoneNumber), Parameter: nameof(ApplicationUser.PhoneNumber)),
                (Rule: IsInvalid(user.Email), Parameter: nameof(ApplicationUser.Email)),
                (Rule: IsInvalid(user.CreatedDate), Parameter: nameof(ApplicationUser.CreatedDate)),
                (Rule: IsInvalid(user.UpdatedDate), Parameter: nameof(ApplicationUser.UpdatedDate)),
                (Rule: IsInvalid(password), Parameter: nameof(ApplicationUser)),

                (Rule: IsNotSame(
                    firstDate: user.UpdatedDate,
                    secondDate: user.CreatedDate,
                    secondDateName: nameof(ApplicationUser.CreatedDate)),
                Parameter: nameof(ApplicationUser.UpdatedDate)),

                (Rule: IsNotRecent(user.UpdatedDate), Parameter: nameof(user.UpdatedDate)));
        }

        private void ValidateUserOnModify(ApplicationUser user)
        {
            ValidateUserIsNull(user);

            Validate(
               (Rule: IsInvalid(user.Id), Parameter: nameof(ApplicationUser.Id)),
               (Rule: IsInvalid(user.Email), Parameter: nameof(ApplicationUser.Email)),
               (Rule: IsInvalid(user.FirstName), Parameter: nameof(ApplicationUser.FirstName)),
               (Rule: IsInvalid(user.LastName), Parameter: nameof(ApplicationUser.LastName)),
               (Rule: IsInvalid(user.UserName), Parameter: nameof(ApplicationUser.UserName)),
               (Rule: IsInvalid(user.PhoneNumber), Parameter: nameof(ApplicationUser.PhoneNumber)),
               (Rule: IsInvalid(user.CreatedDate), Parameter: nameof(ApplicationUser.CreatedDate)),
               (Rule: IsInvalid(user.UpdatedDate), Parameter: nameof(ApplicationUser.UpdatedDate)),

               (Rule: IsSame(
                    firstDate: user.UpdatedDate,
                    secondDate: user.CreatedDate,
                    secondDateName: nameof(user.CreatedDate)),
                Parameter: nameof(user.UpdatedDate)),

                (Rule: IsNotRecent(user.UpdatedDate), Parameter: nameof(user.UpdatedDate)));    
        }

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

        public void ValidateUserId(Guid userId) =>
             Validate((Rule: IsInvalid(userId), Parameter: nameof(ApplicationUser.Id)));

        public void ValidateText(string text) =>
             Validate((Rule: IsInvalid(text), Parameter: nameof(ApplicationUser)));

        private static void ValidateStorageUser(ApplicationUser storageUser, Guid userId)
        {
            if (storageUser is null)
            {
                throw new NotFoundUserException(userId);
            }
        }

        private void ValidateUserOnAddResponse(IdentityResult identityResult)
        {
            if (!identityResult.Succeeded)
            {
                foreach (var errors in identityResult.Errors)
                {
                    Validate(
                         (Rule: IsInvalidIdentity(errors), Parameter: nameof(IdentityError))
                        );
                }
            }

        }

        private void ValidateUserPassword(bool password)
        {
            if (!password)
            {
                throw new UserPasswordValidationException();
            }
        }

        private static void ValidateUserIsNull(ApplicationUser user)
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

        private static dynamic IsInvalidIdentity(IdentityError @object) => new
        {
            Condition = @object is not null,
            Message = @object.Description
        };
        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Value is required"
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

        private static dynamic IsInvalidResult(IdentityError @object) => new
        {
            Condition = @object.Code != null,
            Message = @object.Description
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

        private static void ValidateAgainstStorageUserOnModify(ApplicationUser inputUser, ApplicationUser storageUser)
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
            var invalidUserException = new InvalidUserException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidUserException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidUserException.ThrowIfContainsErrors();
        }
    }
}
