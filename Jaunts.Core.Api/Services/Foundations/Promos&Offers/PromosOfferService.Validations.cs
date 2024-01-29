// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Promos_Offers;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOffers.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.PromosOffers
{
    public partial class PromosOfferService
    {
        private void ValidatePromosOfferOnRegister(PromosOffer PromosOffer)
        {
            ValidatePromosOffer(PromosOffer);

            Validate(
                (Rule: IsInvalid(PromosOffer.Id), Parameter: nameof(PromosOffer.Id)),
                (Rule: IsInvalid(PromosOffer.CodeOrName), Parameter: nameof(PromosOffer.CodeOrName)),
                (Rule: IsInvalid(PromosOffer.Description), Parameter: nameof(PromosOffer.Description)),
                (Rule: IsInvalid(PromosOffer.StartDate), Parameter: nameof(PromosOffer.StartDate)),
                (Rule: IsInvalid(PromosOffer.Status), Parameter: nameof(PromosOffer.Status)),
                (Rule: IsInvalid(PromosOffer.ProviderId), Parameter: nameof(PromosOffer.ProviderId)),
                (Rule: IsInvalid(PromosOffer.Service), Parameter: nameof(PromosOffer.Service)),
                (Rule: IsInvalid(PromosOffer.EndDate), Parameter: nameof(PromosOffer.EndDate)),
                (Rule: IsInvalid(PromosOffer.CreatedBy), Parameter: nameof(PromosOffer.CreatedBy)),
                (Rule: IsInvalid(PromosOffer.UpdatedBy), Parameter: nameof(PromosOffer.UpdatedBy)),
                (Rule: IsInvalid(PromosOffer.CreatedDate), Parameter: nameof(PromosOffer.CreatedDate)),
                (Rule: IsInvalid(PromosOffer.UpdatedDate), Parameter: nameof(PromosOffer.UpdatedDate)),
                (Rule: IsNotRecent(PromosOffer.CreatedDate), Parameter: nameof(PromosOffer.CreatedDate)),

                (Rule: IsNotSame(firstId: PromosOffer.UpdatedBy,
                    secondId: PromosOffer.CreatedBy,
                    secondIdName: nameof(PromosOffer.CreatedBy)),
                    Parameter: nameof(PromosOffer.UpdatedBy)),

                (Rule: IsNotSame(firstDate: PromosOffer.UpdatedDate,
                    secondDate: PromosOffer.CreatedDate,
                    secondDateName: nameof(PromosOffer.CreatedDate)),
                    Parameter: nameof(PromosOffer.UpdatedDate))
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
        private static dynamic IsInvalid(PromosOffersStatus status) => new
        {
            Condition = Enum.IsDefined(status) is false,
            Message = "Value is not recognized"
        };
        private static dynamic IsInvalid(Service status) => new
        {
            Condition = Enum.IsDefined(status) is false,
            Message = "Value is not recognized"
        };
        private static void ValidatePromosOfferId(Guid promosOfferId)
        {
            Validate((Rule: IsInvalid(promosOfferId), Parameter: nameof(PromosOffer.Id)));
        }

        private static void ValidateStoragePromosOffer(PromosOffer storagePromosOffer, Guid promosOfferId)
        {
            if (storagePromosOffer is null)
            {
                throw new NotFoundPromosOffersException(promosOfferId);
            }
        }

        private void ValidatePromosOfferOnModify(PromosOffer PromosOffer)
        {
            ValidatePromosOffer(PromosOffer);

            Validate(
                (Rule: IsInvalid(PromosOffer.Id), Parameter: nameof(PromosOffer.Id)),
                (Rule: IsInvalid(PromosOffer.CodeOrName), Parameter: nameof(PromosOffer.CodeOrName)),
                (Rule: IsInvalid(PromosOffer.Description), Parameter: nameof(PromosOffer.Description)),
                (Rule: IsInvalid(PromosOffer.Status), Parameter: nameof(PromosOffer.Status)),
                (Rule: IsInvalid(PromosOffer.ProviderId), Parameter: nameof(PromosOffer.ProviderId)),
                (Rule: IsInvalid(PromosOffer.Service), Parameter: nameof(PromosOffer.Service)),
                (Rule: IsInvalid(PromosOffer.StartDate), Parameter: nameof(PromosOffer.StartDate)),
                (Rule: IsInvalid(PromosOffer.EndDate), Parameter: nameof(PromosOffer.EndDate)),
                (Rule: IsInvalid(PromosOffer.CreatedBy), Parameter: nameof(PromosOffer.CreatedBy)),
                (Rule: IsInvalid(PromosOffer.UpdatedBy), Parameter: nameof(PromosOffer.UpdatedBy)),
                (Rule: IsInvalid(PromosOffer.CreatedDate), Parameter: nameof(PromosOffer.CreatedDate)),
                (Rule: IsInvalid(PromosOffer.UpdatedDate), Parameter: nameof(PromosOffer.UpdatedDate)),
                (Rule: IsNotRecent(PromosOffer.UpdatedDate), Parameter: nameof(PromosOffer.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: PromosOffer.UpdatedDate,
                    secondDate: PromosOffer.CreatedDate,
                    secondDateName: nameof(PromosOffer.CreatedDate)),
                    Parameter: nameof(PromosOffer.UpdatedDate))
            );
        }

        public void ValidateAgainstStoragePromosOfferOnModify(PromosOffer inputPromosOffer, PromosOffer storagePromosOffer)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputPromosOffer.CreatedDate,
                    secondDate: storagePromosOffer.CreatedDate,
                    secondDateName: nameof(PromosOffer.CreatedDate)),
                    Parameter: nameof(PromosOffer.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputPromosOffer.UpdatedDate,
                    secondDate: storagePromosOffer.UpdatedDate,
                    secondDateName: nameof(PromosOffer.UpdatedDate)),
                    Parameter: nameof(PromosOffer.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: inputPromosOffer.CreatedBy,
                    secondId: storagePromosOffer.CreatedBy,
                    secondIdName: nameof(PromosOffer.CreatedBy)),
                    Parameter: nameof(PromosOffer.CreatedBy))
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

        private static void ValidatePromosOffer(PromosOffer PromosOffer)
        {
            if (PromosOffer is null)
            {
                throw new NullPromosOffersException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidPromosOfferException = new InvalidPromosOffersException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidPromosOfferException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidPromosOfferException.ThrowIfContainsErrors();
        }
    }
}