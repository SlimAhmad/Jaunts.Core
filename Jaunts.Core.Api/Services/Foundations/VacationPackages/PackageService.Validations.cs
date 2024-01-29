// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Drivers;
using Jaunts.Core.Api.Models.Services.Foundations.Packages;
using Jaunts.Core.Api.Models.Services.Foundations.Packages.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.Packages
{
    public partial class PackageService
    {
        private void ValidatePackageOnRegister(Package package)
        {
            ValidatePackage(package);

            Validate(
                (Rule: IsInvalid(package.Id), Parameter: nameof(Package.Id)),
                (Rule: IsInvalid(package.Name), Parameter: nameof(Package.Name)),
                (Rule: IsInvalid(package.Destination), Parameter: nameof(Package.Destination)),
                (Rule: IsInvalid(package.Description), Parameter: nameof(Package.Description)),
                (Rule: IsInvalid(package.StartDate), Parameter: nameof(Package.StartDate)),
                (Rule: IsInvalid(package.EndDate), Parameter: nameof(Package.EndDate)),
                (Rule: IsInvalid(package.Status), Parameter: nameof(Package.Status)),
                (Rule: IsInvalid(package.CreatedBy), Parameter: nameof(Package.CreatedBy)),
                (Rule: IsInvalid(package.UpdatedBy), Parameter: nameof(Package.UpdatedBy)),
                (Rule: IsInvalid(package.CreatedDate), Parameter: nameof(Package.CreatedDate)),
                (Rule: IsInvalid(package.UpdatedDate), Parameter: nameof(Package.UpdatedDate)),
                (Rule: IsNotRecent(package.CreatedDate), Parameter: nameof(Package.CreatedDate)),

                (Rule: IsNotSame(firstId: package.UpdatedBy,
                    secondId: package.CreatedBy,
                    secondIdName: nameof(Package.CreatedBy)),
                    Parameter: nameof(Package.UpdatedBy)),

                (Rule: IsNotSame(firstDate: package.UpdatedDate,
                    secondDate: package.CreatedDate,
                    secondDateName: nameof(Package.CreatedDate)),
                    Parameter: nameof(Package.UpdatedDate))
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

        private static dynamic IsInvalid(PackageStatus status) => new
        {
            Condition = Enum.IsDefined(status) is false,
            Message = "Value is not recognized"
        };

        private static void ValidatePackageId(Guid driverId)
        {
            Validate((Rule: IsInvalid(driverId), Parameter: nameof(Driver.Id)));
        }


        private static void ValidateStoragePackage(Package storagePackage, Guid PackageId)
        {
            if (storagePackage is null)
            {
                throw new NotFoundPackageException(PackageId);
            }
        }

        private void ValidatePackageOnModify(Package Package)
        {
            ValidatePackage(Package);

            Validate(
                (Rule: IsInvalid(Package.Id), Parameter: nameof(Package.Id)),
                (Rule: IsInvalid(Package.Name), Parameter: nameof(Package.Name)),
                (Rule: IsInvalid(Package.Description), Parameter: nameof(Package.Description)),
                (Rule: IsInvalid(Package.StartDate), Parameter: nameof(Package.StartDate)),
                (Rule: IsInvalid(Package.EndDate), Parameter: nameof(Package.EndDate)),
                (Rule: IsInvalid(Package.Status), Parameter: nameof(Package.Status)),
                (Rule: IsInvalid(Package.CreatedBy), Parameter: nameof(Package.CreatedBy)),
                (Rule: IsInvalid(Package.UpdatedBy), Parameter: nameof(Package.UpdatedBy)),
                (Rule: IsInvalid(Package.CreatedDate), Parameter: nameof(Package.CreatedDate)),
                (Rule: IsInvalid(Package.UpdatedDate), Parameter: nameof(Package.UpdatedDate)),
                (Rule: IsNotRecent(Package.UpdatedDate), Parameter: nameof(Package.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: Package.UpdatedDate,
                    secondDate: Package.CreatedDate,
                    secondDateName: nameof(Package.CreatedDate)),
                    Parameter: nameof(Package.UpdatedDate))
            );
        }

        public void ValidateAgainstStoragePackageOnModify(Package inputPackage, Package storagePackage)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputPackage.CreatedDate,
                    secondDate: storagePackage.CreatedDate,
                    secondDateName: nameof(Package.CreatedDate)),
                    Parameter: nameof(Package.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputPackage.UpdatedDate,
                    secondDate: storagePackage.UpdatedDate,
                    secondDateName: nameof(Package.UpdatedDate)),
                    Parameter: nameof(Package.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: inputPackage.CreatedBy,
                    secondId: storagePackage.CreatedBy,
                    secondIdName: nameof(Package.CreatedBy)),
                    Parameter: nameof(Package.CreatedBy))
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

        private static void ValidatePackage(Package Package)
        {
            if (Package is null)
            {
                throw new NullPackageException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidPackageException = new InvalidPackageException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidPackageException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidPackageException.ThrowIfContainsErrors();
        }
    }
}