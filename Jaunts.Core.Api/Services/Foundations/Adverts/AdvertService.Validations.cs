// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Adverts;
using Jaunts.Core.Api.Models.Services.Foundations.Adverts.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.Adverts
{
    public partial class AdvertService
    {
        private void ValidateAdvertOnRegister(Advert advert)
        {
            ValidateAdvert(advert);

            Validate(
                (Rule: IsInvalid(advert.Id), Parameter: nameof(Advert.Id)),
                (Rule: IsInvalid(advert.Description), Parameter: nameof(Advert.Description)),
                (Rule: IsInvalid(advert.EndDate), Parameter: nameof(Advert.EndDate)),
                (Rule: IsInvalid(advert.StartDate), Parameter: nameof(Advert.StartDate)),
                (Rule: IsInvalid(advert.ProviderId), Parameter: nameof(Advert.ProviderId)),
                (Rule: IsInvalid(advert.TransactionFeeId), Parameter: nameof(Advert.TransactionFeeId)),
                (Rule: IsInvalid(advert.CreatedBy), Parameter: nameof(Advert.CreatedBy)),
                (Rule: IsInvalid(advert.UpdatedBy), Parameter: nameof(Advert.UpdatedBy)),
                (Rule: IsInvalid(advert.CreatedDate), Parameter: nameof(Advert.CreatedDate)),
                (Rule: IsInvalid(advert.UpdatedDate), Parameter: nameof(Advert.UpdatedDate)),
                (Rule: IsNotRecent(advert.CreatedDate), Parameter: nameof(Advert.CreatedDate)),

                (Rule: IsNotSame(firstId: advert.UpdatedBy,
                    secondId: advert.CreatedBy,
                    secondIdName: nameof(Advert.CreatedBy)),
                    Parameter: nameof(Advert.UpdatedBy)),

                (Rule: IsNotSame(firstDate: advert.UpdatedDate,
                    secondDate: advert.CreatedDate,
                    secondDateName: nameof(Advert.CreatedDate)),
                    Parameter: nameof(Advert.UpdatedDate))
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

        private static void ValidateAdvertId(Guid advertId)
        {
            if (advertId == Guid.Empty)
            {
                throw new InvalidAdvertException(
                    parameterName: nameof(Advert.Id),
                    parameterValue: advertId);
            }
        }

        private static void ValidateStorageAdvert(Advert storageAdvert, Guid advertId)
        {
            if (storageAdvert is null)
            {
                throw new NotFoundAdvertException(advertId);
            }
        }

        private void ValidateAdvertOnModify(Advert advert)
        {
            ValidateAdvert(advert);

            Validate(
                (Rule: IsInvalid(advert.Id), Parameter: nameof(Advert.Id)),
                (Rule: IsInvalid(advert.Description), Parameter: nameof(Advert.Description)),
                (Rule: IsInvalid(advert.EndDate), Parameter: nameof(Advert.EndDate)),
                (Rule: IsInvalid(advert.StartDate), Parameter: nameof(Advert.StartDate)),
                (Rule: IsInvalid(advert.ProviderId), Parameter: nameof(Advert.ProviderId)),
                (Rule: IsInvalid(advert.TransactionFeeId), Parameter: nameof(Advert.TransactionFeeId)),
                (Rule: IsInvalid(advert.CreatedBy), Parameter: nameof(Advert.CreatedBy)),
                (Rule: IsInvalid(advert.UpdatedBy), Parameter: nameof(Advert.UpdatedBy)),
                (Rule: IsInvalid(advert.CreatedDate), Parameter: nameof(Advert.CreatedDate)),
                (Rule: IsInvalid(advert.UpdatedDate), Parameter: nameof(Advert.UpdatedDate)),
                (Rule: IsNotRecent(advert.UpdatedDate), Parameter: nameof(Advert.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: advert.UpdatedDate,
                    secondDate: advert.CreatedDate,
                    secondDateName: nameof(Advert.CreatedDate)),
                    Parameter: nameof(Advert.UpdatedDate))
            );
        }

        public void ValidateAgainstStorageAdvertOnModify(Advert inputAdvert, Advert storageAdvert)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputAdvert.CreatedDate,
                    secondDate: storageAdvert.CreatedDate,
                    secondDateName: nameof(Advert.CreatedDate)),
                    Parameter: nameof(Advert.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputAdvert.UpdatedDate,
                    secondDate: storageAdvert.UpdatedDate,
                    secondDateName: nameof(Advert.UpdatedDate)),
                    Parameter: nameof(Advert.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: inputAdvert.CreatedBy,
                    secondId: storageAdvert.CreatedBy,
                    secondIdName: nameof(Advert.CreatedBy)),
                    Parameter: nameof(Advert.CreatedBy))
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

        private static void ValidateAdvert(Advert advert)
        {
            if (advert is null)
            {
                throw new NullAdvertException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidAdvertException = new InvalidAdvertException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidAdvertException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidAdvertException.ThrowIfContainsErrors();
        }
    }
}