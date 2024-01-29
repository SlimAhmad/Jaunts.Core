// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Drivers;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategory;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategorys;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategorys.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.ProviderCategories
{
    public partial class ProviderCategoryService
    {
        private void ValidateProviderCategoryOnRegister(ProviderCategory providerCategory)
        {
            ValidateProviderCategory(providerCategory);

            Validate(
                (Rule: IsInvalid(providerCategory.Id), Parameter: nameof(ProviderCategory.Id)),
                (Rule: IsInvalid(providerCategory.Description), Parameter: nameof(ProviderCategory.Description)),
                (Rule: IsInvalid(providerCategory.Name), Parameter: nameof(ProviderCategory.Name)),
                (Rule: IsInvalid(providerCategory.CreatedBy), Parameter: nameof(ProviderCategory.CreatedBy)),
                (Rule: IsInvalid(providerCategory.UpdatedBy), Parameter: nameof(ProviderCategory.UpdatedBy)),
                (Rule: IsInvalid(providerCategory.CreatedDate), Parameter: nameof(ProviderCategory.CreatedDate)),
                (Rule: IsInvalid(providerCategory.UpdatedDate), Parameter: nameof(ProviderCategory.UpdatedDate)),
                (Rule: IsNotRecent(providerCategory.CreatedDate), Parameter: nameof(ProviderCategory.CreatedDate)),

                (Rule: IsNotSame(firstId: providerCategory.UpdatedBy,
                    secondId: providerCategory.CreatedBy,
                    secondIdName: nameof(ProviderCategory.CreatedBy)),
                    Parameter: nameof(ProviderCategory.UpdatedBy)),

                (Rule: IsNotSame(firstDate: providerCategory.UpdatedDate,
                    secondDate: providerCategory.CreatedDate,
                    secondDateName: nameof(ProviderCategory.CreatedDate)),
                    Parameter: nameof(ProviderCategory.UpdatedDate))
            );
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsNotSame(
            Guid firstId,
            Guid secondId,
            string secondIdName) => new
            {
                Condition = firstId != secondId,
                Message = $"Id is not the same as {secondIdName}"
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

        private dynamic IsNotRecent(DateTimeOffset dateTimeOffset) => new
        {
            Condition = IsDateNotRecent(dateTimeOffset),
            Message = "Date is not recent"
        };

        private static void ValidateProviderCategoryId(Guid categoryId)
        {
            Validate((Rule: IsInvalid(categoryId), Parameter: nameof(ProviderCategory.Id)));
        }

        private static void ValidateStorageProviderCategory(ProviderCategory storageProviderCategory, Guid providerCategoryId)
        {
            if (storageProviderCategory is null)
            {
                throw new NotFoundProviderCategoryException(providerCategoryId);
            }
        }

        private void ValidateProviderCategoryOnModify(ProviderCategory providerCategory)
        {
            ValidateProviderCategory(providerCategory);

            Validate(
                (Rule: IsInvalid(providerCategory.Id), Parameter: nameof(ProviderCategory.Id)),
                (Rule: IsInvalid(providerCategory.Description), Parameter: nameof(ProviderCategory.Description)),
                (Rule: IsInvalid(providerCategory.Name), Parameter: nameof(ProviderCategory.Name)),
                (Rule: IsInvalid(providerCategory.CreatedBy), Parameter: nameof(ProviderCategory.CreatedBy)),
                (Rule: IsInvalid(providerCategory.UpdatedBy), Parameter: nameof(ProviderCategory.UpdatedBy)),
                (Rule: IsInvalid(providerCategory.CreatedDate), Parameter: nameof(ProviderCategory.CreatedDate)),
                (Rule: IsInvalid(providerCategory.UpdatedDate), Parameter: nameof(ProviderCategory.UpdatedDate)),
                (Rule: IsNotRecent(providerCategory.UpdatedDate), Parameter: nameof(ProviderCategory.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: providerCategory.UpdatedDate,
                    secondDate: providerCategory.CreatedDate,
                    secondDateName: nameof(ProviderCategory.CreatedDate)),
                    Parameter: nameof(ProviderCategory.UpdatedDate))
            );
        }

        public void ValidateAgainstStorageProviderCategoryOnModify(ProviderCategory inputProviderCategory, ProviderCategory storageProviderCategory)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputProviderCategory.CreatedDate,
                    secondDate: storageProviderCategory.CreatedDate,
                    secondDateName: nameof(ProviderCategory.CreatedDate)),
                    Parameter: nameof(ProviderCategory.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputProviderCategory.UpdatedDate,
                    secondDate: storageProviderCategory.UpdatedDate,
                    secondDateName: nameof(ProviderCategory.UpdatedDate)),
                    Parameter: nameof(ProviderCategory.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: inputProviderCategory.CreatedBy,
                    secondId: storageProviderCategory.CreatedBy,
                    secondIdName: nameof(ProviderCategory.CreatedBy)),
                    Parameter: nameof(ProviderCategory.CreatedBy))
            );
        }

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTime();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

        private static void ValidateProviderCategory(ProviderCategory providerCategory)
        {
            if (providerCategory is null)
            {
                throw new NullProviderCategoryException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidProviderCategoryException = new InvalidProviderCategoryException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidProviderCategoryException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidProviderCategoryException.ThrowIfContainsErrors();
        }
    }
}