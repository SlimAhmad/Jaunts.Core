// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.VacationPackages;
using Jaunts.Core.Api.Models.Services.Foundations.VacationPackages.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.VacationPackages
{
    public partial class VacationPackageService
    {
        private void ValidateVacationPackageOnRegister(VacationPackage vacationPackage)
        {
            ValidateVacationPackage(vacationPackage);

            Validate(
                (Rule: IsInvalid(vacationPackage.Id), Parameter: nameof(VacationPackage.Id)),
                (Rule: IsInvalid(vacationPackage.Name), Parameter: nameof(VacationPackage.Name)),
                (Rule: IsInvalid(vacationPackage.Destination), Parameter: nameof(VacationPackage.Destination)),
                (Rule: IsInvalid(vacationPackage.Description), Parameter: nameof(VacationPackage.Description)),
                (Rule: IsInvalid(vacationPackage.CreatedBy), Parameter: nameof(VacationPackage.CreatedBy)),
                (Rule: IsInvalid(vacationPackage.UpdatedBy), Parameter: nameof(VacationPackage.UpdatedBy)),
                (Rule: IsInvalid(vacationPackage.CreatedDate), Parameter: nameof(VacationPackage.CreatedDate)),
                (Rule: IsInvalid(vacationPackage.UpdatedDate), Parameter: nameof(VacationPackage.UpdatedDate)),
                (Rule: IsNotRecent(vacationPackage.CreatedDate), Parameter: nameof(VacationPackage.CreatedDate)),

                (Rule: IsNotSame(firstId: vacationPackage.UpdatedBy,
                    secondId: vacationPackage.CreatedBy,
                    secondIdName: nameof(VacationPackage.CreatedBy)),
                    Parameter: nameof(VacationPackage.UpdatedBy)),

                (Rule: IsNotSame(firstDate: vacationPackage.UpdatedDate,
                    secondDate: vacationPackage.CreatedDate,
                    secondDateName: nameof(VacationPackage.CreatedDate)),
                    Parameter: nameof(VacationPackage.UpdatedDate))
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

        private static void ValidateVacationPackageId(Guid vacationPackageId)
        {
            if (vacationPackageId == Guid.Empty)
            {
                throw new InvalidVacationPackageException(
                    parameterName: nameof(VacationPackage.Id),
                    parameterValue: vacationPackageId);
            }
        }

        private static void ValidateStorageVacationPackage(VacationPackage storageVacationPackage, Guid vacationPackageId)
        {
            if (storageVacationPackage is null)
            {
                throw new NotFoundVacationPackageException(vacationPackageId);
            }
        }

        private void ValidateVacationPackageOnModify(VacationPackage vacationPackage)
        {
            ValidateVacationPackage(vacationPackage);

            Validate(
                (Rule: IsInvalid(vacationPackage.Id), Parameter: nameof(VacationPackage.Id)),
                (Rule: IsInvalid(vacationPackage.Name), Parameter: nameof(VacationPackage.Name)),
                (Rule: IsInvalid(vacationPackage.Description), Parameter: nameof(VacationPackage.Description)),
                (Rule: IsInvalid(vacationPackage.CreatedBy), Parameter: nameof(VacationPackage.CreatedBy)),
                (Rule: IsInvalid(vacationPackage.UpdatedBy), Parameter: nameof(VacationPackage.UpdatedBy)),
                (Rule: IsInvalid(vacationPackage.CreatedDate), Parameter: nameof(VacationPackage.CreatedDate)),
                (Rule: IsInvalid(vacationPackage.UpdatedDate), Parameter: nameof(VacationPackage.UpdatedDate)),
                (Rule: IsNotRecent(vacationPackage.UpdatedDate), Parameter: nameof(VacationPackage.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: vacationPackage.UpdatedDate,
                    secondDate: vacationPackage.CreatedDate,
                    secondDateName: nameof(VacationPackage.CreatedDate)),
                    Parameter: nameof(VacationPackage.UpdatedDate))
            );
        }

        public void ValidateAgainstStorageVacationPackageOnModify(VacationPackage inputVacationPackage, VacationPackage storageVacationPackage)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputVacationPackage.CreatedDate,
                    secondDate: storageVacationPackage.CreatedDate,
                    secondDateName: nameof(VacationPackage.CreatedDate)),
                    Parameter: nameof(VacationPackage.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputVacationPackage.UpdatedDate,
                    secondDate: storageVacationPackage.UpdatedDate,
                    secondDateName: nameof(VacationPackage.UpdatedDate)),
                    Parameter: nameof(VacationPackage.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: inputVacationPackage.CreatedBy,
                    secondId: storageVacationPackage.CreatedBy,
                    secondIdName: nameof(VacationPackage.CreatedBy)),
                    Parameter: nameof(VacationPackage.CreatedBy))
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

        private static void ValidateVacationPackage(VacationPackage vacationPackage)
        {
            if (vacationPackage is null)
            {
                throw new NullVacationPackageException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidVacationPackageException = new InvalidVacationPackageException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidVacationPackageException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidVacationPackageException.ThrowIfContainsErrors();
        }
    }
}