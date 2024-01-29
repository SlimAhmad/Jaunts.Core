// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Adverts;
using Jaunts.Core.Api.Models.Services.Foundations.Drivers;
using Jaunts.Core.Api.Models.Services.Foundations.Drivers.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Drivers;

namespace Jaunts.Core.Api.Services.Foundations.Drivers
{
    public partial class DriverService
    {
        private void ValidateDriverOnRegister(Driver driver)
        {
            ValidateDriver(driver);

            Validate(
                (Rule: IsInvalid(driver.Id), Parameter: nameof(Driver.Id)),
                (Rule: IsInvalid(driver.LicenseNumber), Parameter: nameof(Driver.LicenseNumber)),
                (Rule: IsInvalid(driver.FirstName), Parameter: nameof(Driver.FirstName)),
                (Rule: IsInvalid(driver.LastName), Parameter: nameof(Driver.LastName)),
                (Rule: IsInvalid(driver.MiddleName), Parameter: nameof(Driver.MiddleName)),
                (Rule: IsInvalid(driver.ContactNumber), Parameter: nameof(Driver.ContactNumber)),
                (Rule: IsInvalid(driver.DriverStatus), Parameter: nameof(Driver.DriverStatus)),
                (Rule: IsInvalid(driver.FleetId), Parameter: nameof(Driver.FleetId)),
                (Rule: IsInvalid(driver.ProviderId), Parameter: nameof(Driver.ProviderId)),
                (Rule: IsInvalid(driver.CreatedBy), Parameter: nameof(Driver.CreatedBy)),
                (Rule: IsInvalid(driver.UpdatedBy), Parameter: nameof(Driver.UpdatedBy)),
                (Rule: IsInvalid(driver.CreatedDate), Parameter: nameof(Driver.CreatedDate)),
                (Rule: IsInvalid(driver.UpdatedDate), Parameter: nameof(Driver.UpdatedDate)),
                (Rule: IsNotRecent(driver.CreatedDate), Parameter: nameof(Driver.CreatedDate)),

                (Rule: IsNotSame(firstId: driver.UpdatedBy,
                    secondId: driver.CreatedBy,
                    secondIdName: nameof(Driver.CreatedBy)),
                    Parameter: nameof(Driver.UpdatedBy)),

                (Rule: IsNotSame(firstDate: driver.UpdatedDate,
                    secondDate: driver.CreatedDate,
                    secondDateName: nameof(Driver.CreatedDate)),
                    Parameter: nameof(Driver.UpdatedDate))
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

        private static dynamic IsInvalid(DriverStatus status) => new
        {
            Condition = Enum.IsDefined(status) is false,
            Message = "Value is not recognized"
        };

        private static void ValidateDriverId(Guid driverId)
        {
            Validate((Rule: IsInvalid(driverId), Parameter: nameof(Driver.Id)));
        }

        private static void ValidateStorageDriver(Driver storageDriver, Guid driverId)
        {
            if (storageDriver is null)
            {
                throw new NotFoundDriverException(driverId);
            }
        }

        private void ValidateDriverOnModify(Driver driver)
        {
            ValidateDriver(driver);

            Validate(
                (Rule: IsInvalid(driver.Id), Parameter: nameof(Driver.Id)),
                (Rule: IsInvalid(driver.LicenseNumber), Parameter: nameof(Driver.LicenseNumber)),
                (Rule: IsInvalid(driver.FirstName), Parameter: nameof(Driver.FirstName)),
                (Rule: IsInvalid(driver.LastName), Parameter: nameof(Driver.LastName)),
                (Rule: IsInvalid(driver.MiddleName), Parameter: nameof(Driver.MiddleName)),
                (Rule: IsInvalid(driver.ContactNumber), Parameter: nameof(Driver.ContactNumber)),
                (Rule: IsInvalid(driver.DriverStatus), Parameter: nameof(Driver.DriverStatus)),
                (Rule: IsInvalid(driver.ProviderId), Parameter: nameof(Driver.ProviderId)),
                (Rule: IsInvalid(driver.FleetId), Parameter: nameof(Driver.FleetId)),
                (Rule: IsInvalid(driver.CreatedBy), Parameter: nameof(Driver.CreatedBy)),
                (Rule: IsInvalid(driver.UpdatedBy), Parameter: nameof(Driver.UpdatedBy)),
                (Rule: IsInvalid(driver.CreatedDate), Parameter: nameof(Driver.CreatedDate)),
                (Rule: IsInvalid(driver.UpdatedDate), Parameter: nameof(Driver.UpdatedDate)),
                (Rule: IsNotRecent(driver.UpdatedDate), Parameter: nameof(Driver.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: driver.UpdatedDate,
                    secondDate: driver.CreatedDate,
                    secondDateName: nameof(Driver.CreatedDate)),
                    Parameter: nameof(Driver.UpdatedDate))
            );
        }

        public void ValidateAgainstStorageDriverOnModify(Driver inputDriver, Driver storageDriver)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputDriver.CreatedDate,
                    secondDate: storageDriver.CreatedDate,
                    secondDateName: nameof(Driver.CreatedDate)),
                    Parameter: nameof(Driver.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputDriver.UpdatedDate,
                    secondDate: storageDriver.UpdatedDate,
                    secondDateName: nameof(Driver.UpdatedDate)),
                    Parameter: nameof(Driver.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: inputDriver.CreatedBy,
                    secondId: storageDriver.CreatedBy,
                    secondIdName: nameof(Driver.CreatedBy)),
                    Parameter: nameof(Driver.CreatedBy))
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

        private static void ValidateDriver(Driver driver)
        {
            if (driver is null)
            {
                throw new NullDriverException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidDriverException = new InvalidDriverException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDriverException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDriverException.ThrowIfContainsErrors();
        }
    }
}