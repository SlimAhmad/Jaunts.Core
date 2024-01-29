// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Drivers;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLets;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLets.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.ShortLets
{
    public partial class ShortLetService
    {
        private void ValidateShortLetOnRegister(ShortLet shortLet)
        {
            ValidateShortLet(shortLet);

            Validate(
                (Rule: IsInvalid(shortLet.Id), Parameter: nameof(ShortLet.Id)),
                (Rule: IsInvalid(shortLet.Name), Parameter: nameof(ShortLet.Name)),
                (Rule: IsInvalid(shortLet.Location), Parameter: nameof(ShortLet.Location)),
                (Rule: IsInvalid(shortLet.Description), Parameter: nameof(ShortLet.Description)),
                (Rule: IsInvalid(shortLet.Status), Parameter: nameof(ShortLet.Status)),
                (Rule: IsInvalid(shortLet.CreatedBy), Parameter: nameof(ShortLet.CreatedBy)),
                (Rule: IsInvalid(shortLet.UpdatedBy), Parameter: nameof(ShortLet.UpdatedBy)),
                (Rule: IsInvalid(shortLet.CreatedDate), Parameter: nameof(ShortLet.CreatedDate)),
                (Rule: IsInvalid(shortLet.UpdatedDate), Parameter: nameof(ShortLet.UpdatedDate)),
                (Rule: IsNotRecent(shortLet.CreatedDate), Parameter: nameof(ShortLet.CreatedDate)),

                (Rule: IsNotSame(firstId: shortLet.UpdatedBy,
                    secondId: shortLet.CreatedBy,
                    secondIdName: nameof(ShortLet.CreatedBy)),
                    Parameter: nameof(ShortLet.UpdatedBy)),

                (Rule: IsNotSame(firstDate: shortLet.UpdatedDate,
                    secondDate: shortLet.CreatedDate,
                    secondDateName: nameof(ShortLet.CreatedDate)),
                    Parameter: nameof(ShortLet.UpdatedDate))
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

        private static dynamic IsInvalid(ShortLetStatus status) => new
        {
            Condition = Enum.IsDefined(status) is false,
            Message = "Value is not recognized"
        };

        private static void ValidateShortLetId(Guid shortLetId)
        {
            Validate((Rule: IsInvalid(shortLetId), Parameter: nameof(ShortLet.Id)));
        }
        private static void ValidateStorageShortLet(ShortLet storageShortLet, Guid shortLetId)
        {
            if (storageShortLet is null)
            {
                throw new NotFoundShortLetException(shortLetId);
            }
        }

        private void ValidateShortLetOnModify(ShortLet shortLet)
        {
            ValidateShortLet(shortLet);

            Validate(
                (Rule: IsInvalid(shortLet.Id), Parameter: nameof(ShortLet.Id)),
                (Rule: IsInvalid(shortLet.Name), Parameter: nameof(ShortLet.Name)),
                (Rule: IsInvalid(shortLet.Description), Parameter: nameof(ShortLet.Description)),
                (Rule: IsInvalid(shortLet.Location), Parameter: nameof(ShortLet.Location)),
                (Rule: IsInvalid(shortLet.Status), Parameter: nameof(ShortLet.Status)),
                (Rule: IsInvalid(shortLet.CreatedBy), Parameter: nameof(ShortLet.CreatedBy)),
                (Rule: IsInvalid(shortLet.UpdatedBy), Parameter: nameof(ShortLet.UpdatedBy)),
                (Rule: IsInvalid(shortLet.CreatedDate), Parameter: nameof(ShortLet.CreatedDate)),
                (Rule: IsInvalid(shortLet.UpdatedDate), Parameter: nameof(ShortLet.UpdatedDate)),
                (Rule: IsNotRecent(shortLet.UpdatedDate), Parameter: nameof(ShortLet.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: shortLet.UpdatedDate,
                    secondDate: shortLet.CreatedDate,
                    secondDateName: nameof(ShortLet.CreatedDate)),
                    Parameter: nameof(ShortLet.UpdatedDate))
            );
        }

        public void ValidateAgainstStorageShortLetOnModify(ShortLet inputShortLet, ShortLet storageShortLet)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputShortLet.CreatedDate,
                    secondDate: storageShortLet.CreatedDate,
                    secondDateName: nameof(ShortLet.CreatedDate)),
                    Parameter: nameof(ShortLet.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputShortLet.UpdatedDate,
                    secondDate: storageShortLet.UpdatedDate,
                    secondDateName: nameof(ShortLet.UpdatedDate)),
                    Parameter: nameof(ShortLet.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: inputShortLet.CreatedBy,
                    secondId: storageShortLet.CreatedBy,
                    secondIdName: nameof(ShortLet.CreatedBy)),
                    Parameter: nameof(ShortLet.CreatedBy))
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

        private static void ValidateShortLet(ShortLet shortLet)
        {
            if (shortLet is null)
            {
                throw new NullShortLetException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidShortLetException = new InvalidShortLetException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidShortLetException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidShortLetException.ThrowIfContainsErrors();
        }
    }
}