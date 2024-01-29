// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Fleets;
using Jaunts.Core.Api.Models.Services.Foundations.Fleets;
using Jaunts.Core.Api.Models.Services.Foundations.Fleets.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.Fleets
{
    public partial class FleetService
    {
        private void ValidateFleetOnRegister(Fleet fleet)
        {
            ValidateFleet(fleet);

            Validate(
                (Rule: IsInvalid(fleet.Id), Parameter: nameof(Fleet.Id)),
                (Rule: IsInvalid(fleet.PlateNumber), Parameter: nameof(Fleet.PlateNumber)),
                (Rule: IsInvalid(fleet.Name), Parameter: nameof(Fleet.Name)),
                (Rule: IsInvalid(fleet.TransmissionType), Parameter: nameof(Fleet.TransmissionType)),
                (Rule: IsInvalid(fleet.FuelType), Parameter: nameof(Fleet.FuelType)),
                (Rule: IsInvalid(fleet.Description), Parameter: nameof(Fleet.Description)),
                (Rule: IsInvalid(fleet.ProviderId), Parameter: nameof(Fleet.ProviderId)),
                (Rule: IsInvalid(fleet.Status), Parameter: nameof(Fleet.Status)),
                (Rule: IsInvalid(fleet.Model), Parameter: nameof(Fleet.Model)),
                (Rule: IsInvalid(fleet.CreatedBy), Parameter: nameof(Fleet.CreatedBy)),
                (Rule: IsInvalid(fleet.UpdatedBy), Parameter: nameof(Fleet.UpdatedBy)),
                (Rule: IsInvalid(fleet.CreatedDate), Parameter: nameof(Fleet.CreatedDate)),
                (Rule: IsInvalid(fleet.UpdatedDate), Parameter: nameof(Fleet.UpdatedDate)),
                (Rule: IsNotRecent(fleet.CreatedDate), Parameter: nameof(Fleet.CreatedDate)),

                (Rule: IsNotSame(firstId: fleet.UpdatedBy,
                    secondId: fleet.CreatedBy,
                    secondIdName: nameof(Fleet.CreatedBy)),
                    Parameter: nameof(Fleet.UpdatedBy)),

                (Rule: IsNotSame(firstDate: fleet.UpdatedDate,
                    secondDate: fleet.CreatedDate,
                    secondDateName: nameof(Fleet.CreatedDate)),
                    Parameter: nameof(Fleet.UpdatedDate))
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

        private static dynamic IsInvalid(FleetStatus status) => new
        {
            Condition = Enum.IsDefined(status) is false,
            Message = "Value is not recognized"
        };

        private static void ValidateFleetId(Guid fleetId)
        {
            Validate((Rule: IsInvalid(fleetId), Parameter: nameof(Fleet.Id)));
        }

        private static void ValidateStorageFleet(Fleet storageFleet, Guid fleetId)
        {
            if (storageFleet is null)
            {
                throw new NotFoundFleetException(fleetId);
            }
        }

        private void ValidateFleetOnModify(Fleet fleet)
        {
            ValidateFleet(fleet);

            Validate(
                (Rule: IsInvalid(fleet.Id), Parameter: nameof(Fleet.Id)),
                (Rule: IsInvalid(fleet.PlateNumber), Parameter: nameof(Fleet.PlateNumber)),
                (Rule: IsInvalid(fleet.Name), Parameter: nameof(Fleet.Name)),
                (Rule: IsInvalid(fleet.TransmissionType), Parameter: nameof(Fleet.TransmissionType)),
                (Rule: IsInvalid(fleet.FuelType), Parameter: nameof(Fleet.FuelType)),
                (Rule: IsInvalid(fleet.Description), Parameter: nameof(Fleet.Description)),
                (Rule: IsInvalid(fleet.ProviderId), Parameter: nameof(Fleet.ProviderId)),
                (Rule: IsInvalid(fleet.Status), Parameter: nameof(Fleet.Status)),
                (Rule: IsInvalid(fleet.Model), Parameter: nameof(Fleet.Model)),
                (Rule: IsInvalid(fleet.CreatedBy), Parameter: nameof(Fleet.CreatedBy)),
                (Rule: IsInvalid(fleet.UpdatedBy), Parameter: nameof(Fleet.UpdatedBy)),
                (Rule: IsInvalid(fleet.CreatedDate), Parameter: nameof(Fleet.CreatedDate)),
                (Rule: IsInvalid(fleet.UpdatedDate), Parameter: nameof(Fleet.UpdatedDate)),
                (Rule: IsNotRecent(fleet.UpdatedDate), Parameter: nameof(Fleet.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: fleet.UpdatedDate,
                    secondDate: fleet.CreatedDate,
                    secondDateName: nameof(Fleet.CreatedDate)),
                    Parameter: nameof(Fleet.UpdatedDate))
            );
        }

        public void ValidateAgainstStorageFleetOnModify(Fleet inputFleet, Fleet storageFleet)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputFleet.CreatedDate,
                    secondDate: storageFleet.CreatedDate,
                    secondDateName: nameof(Fleet.CreatedDate)),
                    Parameter: nameof(Fleet.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputFleet.UpdatedDate,
                    secondDate: storageFleet.UpdatedDate,
                    secondDateName: nameof(Fleet.UpdatedDate)),
                    Parameter: nameof(Fleet.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: inputFleet.CreatedBy,
                    secondId: storageFleet.CreatedBy,
                    secondIdName: nameof(Fleet.CreatedBy)),
                    Parameter: nameof(Fleet.CreatedBy))
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

        private static void ValidateFleet(Fleet fleet)
        {
            if (fleet is null)
            {
                throw new NullFleetException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidFleetException = new InvalidFleetException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidFleetException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidFleetException.ThrowIfContainsErrors();
        }
    }
}