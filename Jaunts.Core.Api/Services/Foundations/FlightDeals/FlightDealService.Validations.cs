// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.FlightDeals
{
    public partial class FlightDealService
    {
        private void ValidateFlightDealOnRegister(FlightDeal flightDeal)
        {
            ValidateFlightDeal(flightDeal);

            Validate(
                (Rule: IsInvalid(flightDeal.Id), Parameter: nameof(FlightDeal.Id)),
                (Rule: IsInvalid(flightDeal.Airline), Parameter: nameof(FlightDeal.Airline)),
                (Rule: IsInvalid(flightDeal.ArrivalCity), Parameter: nameof(FlightDeal.ArrivalCity)),
                (Rule: IsInvalid(flightDeal.Description), Parameter: nameof(FlightDeal.Description)),
                (Rule: IsInvalid(flightDeal.DepartureCity), Parameter: nameof(FlightDeal.DepartureCity)),
                (Rule: IsInvalid(flightDeal.EndDate), Parameter: nameof(FlightDeal.EndDate)),
                (Rule: IsInvalid(flightDeal.StartDate), Parameter: nameof(FlightDeal.StartDate)),
                (Rule: IsInvalid(flightDeal.CreatedBy), Parameter: nameof(FlightDeal.CreatedBy)),
                (Rule: IsInvalid(flightDeal.UpdatedBy), Parameter: nameof(FlightDeal.UpdatedBy)),
                (Rule: IsInvalid(flightDeal.CreatedDate), Parameter: nameof(FlightDeal.CreatedDate)),
                (Rule: IsInvalid(flightDeal.UpdatedDate), Parameter: nameof(FlightDeal.UpdatedDate)),
                (Rule: IsNotRecent(flightDeal.CreatedDate), Parameter: nameof(FlightDeal.CreatedDate)),

                (Rule: IsNotSame(firstId: flightDeal.UpdatedBy,
                    secondId: flightDeal.CreatedBy,
                    secondIdName: nameof(FlightDeal.CreatedBy)),
                    Parameter: nameof(FlightDeal.UpdatedBy)),

                (Rule: IsNotSame(firstDate: flightDeal.UpdatedDate,
                    secondDate: flightDeal.CreatedDate,
                    secondDateName: nameof(FlightDeal.CreatedDate)),
                    Parameter: nameof(FlightDeal.UpdatedDate))
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

        private static void ValidateFlightDealId(Guid flightDealId)
        {
            if (flightDealId == Guid.Empty)
            {
                throw new InvalidFlightDealException(
                    parameterName: nameof(FlightDeal.Id),
                    parameterValue: flightDealId);
            }
        }

        private static void ValidateStorageFlightDeal(FlightDeal storageFlightDeal, Guid flightDealId)
        {
            if (storageFlightDeal is null)
            {
                throw new NotFoundFlightDealException(flightDealId);
            }
        }

        private void ValidateFlightDealOnModify(FlightDeal flightDeal)
        {
            ValidateFlightDeal(flightDeal);

            Validate(
                (Rule: IsInvalid(flightDeal.Id), Parameter: nameof(FlightDeal.Id)),
                (Rule: IsInvalid(flightDeal.Airline), Parameter: nameof(FlightDeal.Airline)),
                (Rule: IsInvalid(flightDeal.ArrivalCity), Parameter: nameof(FlightDeal.ArrivalCity)),
                (Rule: IsInvalid(flightDeal.Description), Parameter: nameof(FlightDeal.Description)),
                (Rule: IsInvalid(flightDeal.DepartureCity), Parameter: nameof(FlightDeal.DepartureCity)),
                (Rule: IsInvalid(flightDeal.EndDate), Parameter: nameof(FlightDeal.EndDate)),
                (Rule: IsInvalid(flightDeal.StartDate), Parameter: nameof(FlightDeal.StartDate)),
                (Rule: IsInvalid(flightDeal.CreatedBy), Parameter: nameof(FlightDeal.CreatedBy)),
                (Rule: IsInvalid(flightDeal.UpdatedBy), Parameter: nameof(FlightDeal.UpdatedBy)),
                (Rule: IsInvalid(flightDeal.CreatedDate), Parameter: nameof(FlightDeal.CreatedDate)),
                (Rule: IsInvalid(flightDeal.UpdatedDate), Parameter: nameof(FlightDeal.UpdatedDate)),
                (Rule: IsNotRecent(flightDeal.UpdatedDate), Parameter: nameof(FlightDeal.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: flightDeal.UpdatedDate,
                    secondDate: flightDeal.CreatedDate,
                    secondDateName: nameof(FlightDeal.CreatedDate)),
                    Parameter: nameof(FlightDeal.UpdatedDate))
            );
        }

        public void ValidateAgainstStorageFlightDealOnModify(FlightDeal inputFlightDeal, FlightDeal storageFlightDeal)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputFlightDeal.CreatedDate,
                    secondDate: storageFlightDeal.CreatedDate,
                    secondDateName: nameof(FlightDeal.CreatedDate)),
                    Parameter: nameof(FlightDeal.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputFlightDeal.UpdatedDate,
                    secondDate: storageFlightDeal.UpdatedDate,
                    secondDateName: nameof(FlightDeal.UpdatedDate)),
                    Parameter: nameof(FlightDeal.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: inputFlightDeal.CreatedBy,
                    secondId: storageFlightDeal.CreatedBy,
                    secondIdName: nameof(FlightDeal.CreatedBy)),
                    Parameter: nameof(FlightDeal.CreatedBy))
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

        private static void ValidateFlightDeal(FlightDeal flightDeal)
        {
            if (flightDeal is null)
            {
                throw new NullFlightDealException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidFlightDealException = new InvalidFlightDealException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidFlightDealException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidFlightDealException.ThrowIfContainsErrors();
        }
    }
}