// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Rides;
using Jaunts.Core.Api.Models.Services.Foundations.Rides.Exceptions;
using Termii.Core.Models.Services.Foundations.Termii.Tokens;

namespace Jaunts.Core.Api.Services.Foundations.Rides
{
    public partial class RideService
    {
        private void ValidateRideOnRegister(Ride ride)
        {
            ValidateRide(ride);

            Validate(
                (Rule: IsInvalid(ride.Id), Parameter: nameof(Ride.Id)),
                (Rule: IsInvalid(ride.Description), Parameter: nameof(Ride.Description)),
                (Rule: IsInvalid(ride.Location), Parameter: nameof(Ride.Location)),
                (Rule: IsInvalid(ride.Name), Parameter: nameof(Ride.Name)),
                (Rule: IsInvalid(ride.RideStatus), Parameter: nameof(Ride.RideStatus)),
                (Rule: IsInvalid(ride.FleetId), Parameter: nameof(Ride.FleetId)),
                (Rule: IsInvalid(ride.CreatedBy), Parameter: nameof(Ride.CreatedBy)),
                (Rule: IsInvalid(ride.UpdatedBy), Parameter: nameof(Ride.UpdatedBy)),
                (Rule: IsInvalid(ride.CreatedDate), Parameter: nameof(Ride.CreatedDate)),
                (Rule: IsInvalid(ride.UpdatedDate), Parameter: nameof(Ride.UpdatedDate)),
                (Rule: IsNotRecent(ride.CreatedDate), Parameter: nameof(Ride.CreatedDate)),

                (Rule: IsNotSame(firstId: ride.UpdatedBy,
                    secondId: ride.CreatedBy,
                    secondIdName: nameof(Ride.CreatedBy)),
                    Parameter: nameof(Ride.UpdatedBy)),

                (Rule: IsNotSame(firstDate: ride.UpdatedDate,
                    secondDate: ride.CreatedDate,
                    secondDateName: nameof(Ride.CreatedDate)),
                    Parameter: nameof(Ride.UpdatedDate))
            );
        }

        private void ValidateRideOnModify(Ride ride)
        {
            ValidateRide(ride);

            Validate(
                (Rule: IsInvalid(ride.Id), Parameter: nameof(Ride.Id)),
                (Rule: IsInvalid(ride.Description), Parameter: nameof(Ride.Description)),
                (Rule: IsInvalid(ride.Location), Parameter: nameof(Ride.Location)),
                (Rule: IsInvalid(ride.Name), Parameter: nameof(Ride.Name)),
                (Rule: IsInvalid(ride.RideStatus), Parameter: nameof(Ride.RideStatus)),
                (Rule: IsInvalid(ride.FleetId), Parameter: nameof(Ride.FleetId)),
                (Rule: IsInvalid(ride.CreatedBy), Parameter: nameof(Ride.CreatedBy)),
                (Rule: IsInvalid(ride.UpdatedBy), Parameter: nameof(Ride.UpdatedBy)),
                (Rule: IsInvalid(ride.CreatedDate), Parameter: nameof(Ride.CreatedDate)),
                (Rule: IsNotRecent(ride.UpdatedDate), Parameter: nameof(Ride.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: ride.UpdatedDate,
                    secondDate: ride.CreatedDate,
                    secondDateName: nameof(Ride.CreatedDate)),
                    Parameter: nameof(Ride.UpdatedDate))
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

        private static dynamic IsInvalid(RideStatus status) => new
        {
            Condition = Enum.IsDefined(status) is false,
            Message = "Value is not recognized"
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

        private static void ValidateRideId(Guid rideId)
        {
            Validate((Rule: IsInvalid(rideId), Parameter: nameof(Ride.Id)));
        }

        private static void ValidateStorageRide(Ride storageRide, Guid rideId)
        {
            if (storageRide is null)
            {
                throw new NotFoundRideException(rideId);
            }
        }

        public void ValidateAgainstStorageRideOnModify(Ride inputRide, Ride storageRide)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputRide.CreatedDate,
                    secondDate: storageRide.CreatedDate,
                    secondDateName: nameof(Ride.CreatedDate)),
                    Parameter: nameof(Ride.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputRide.UpdatedDate,
                    secondDate: storageRide.UpdatedDate,
                    secondDateName: nameof(Ride.UpdatedDate)),
                    Parameter: nameof(Ride.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: inputRide.CreatedBy,
                    secondId: storageRide.CreatedBy,
                    secondIdName: nameof(Ride.CreatedBy)),
                    Parameter: nameof(Ride.CreatedBy))
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

        private static void ValidateRide(Ride ride)
        {
            if (ride is null)
            {
                throw new NullRideException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidRideException = new InvalidRideException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidRideException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidRideException.ThrowIfContainsErrors();
        }
    }
}