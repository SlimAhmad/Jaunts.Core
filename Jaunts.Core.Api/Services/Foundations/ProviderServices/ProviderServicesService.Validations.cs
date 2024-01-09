// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.ProviderServices;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderServices.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.ProviderServices
{
    public partial class ProviderServicesService
    {
        private void ValidateProviderServiceOnRegister(ProviderService providerService)
        {
            ValidateProviderService(providerService);

            Validate(
                (Rule: IsInvalid(providerService.Id), Parameter: nameof(ProviderService.Id)),
                (Rule: IsInvalid(providerService.ServiceName), Parameter: nameof(ProviderService.ServiceName)),
                (Rule: IsInvalid(providerService.ProviderId), Parameter: nameof(ProviderService.ProviderId)),
                (Rule: IsInvalid(providerService.Description), Parameter: nameof(ProviderService.Description)),
                (Rule: IsInvalid(providerService.CreatedBy), Parameter: nameof(ProviderService.CreatedBy)),
                (Rule: IsInvalid(providerService.UpdatedBy), Parameter: nameof(ProviderService.UpdatedBy)),
                (Rule: IsInvalid(providerService.CreatedDate), Parameter: nameof(ProviderService.CreatedDate)),
                (Rule: IsInvalid(providerService.UpdatedDate), Parameter: nameof(ProviderService.UpdatedDate)),
                (Rule: IsNotRecent(providerService.CreatedDate), Parameter: nameof(ProviderService.CreatedDate)),

                (Rule: IsNotSame(firstId: providerService.UpdatedBy,
                    secondId: providerService.CreatedBy,
                    secondIdName: nameof(ProviderService.CreatedBy)),
                    Parameter: nameof(ProviderService.UpdatedBy)),

                (Rule: IsNotSame(firstDate: providerService.UpdatedDate,
                    secondDate: providerService.CreatedDate,
                    secondDateName: nameof(ProviderService.CreatedDate)),
                    Parameter: nameof(ProviderService.UpdatedDate))
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

        private static void ValidateProviderServiceId(Guid providerServiceId)
        {
            if (providerServiceId == Guid.Empty)
            {
                throw new InvalidProviderServiceException(
                    parameterName: nameof(ProviderService.Id),
                    parameterValue: providerServiceId);
            }
        }

        private static void ValidateStorageProviderService(ProviderService storageProviderService, Guid providerServiceId)
        {
            if (storageProviderService is null)
            {
                throw new NotFoundProviderServiceException(providerServiceId);
            }
        }

        private void ValidateProviderServiceOnModify(ProviderService providerService)
        {
            ValidateProviderService(providerService);

            Validate(
                (Rule: IsInvalid(providerService.Id), Parameter: nameof(ProviderService.Id)),
                (Rule: IsInvalid(providerService.ServiceName), Parameter: nameof(ProviderService.ServiceName)),
                (Rule: IsInvalid(providerService.ProviderId), Parameter: nameof(ProviderService.ProviderId)),
                (Rule: IsInvalid(providerService.Description), Parameter: nameof(ProviderService.Description)),
                (Rule: IsInvalid(providerService.CreatedBy), Parameter: nameof(ProviderService.CreatedBy)),
                (Rule: IsInvalid(providerService.UpdatedBy), Parameter: nameof(ProviderService.UpdatedBy)),
                (Rule: IsInvalid(providerService.CreatedDate), Parameter: nameof(ProviderService.CreatedDate)),
                (Rule: IsInvalid(providerService.UpdatedDate), Parameter: nameof(ProviderService.UpdatedDate)),
                (Rule: IsNotRecent(providerService.UpdatedDate), Parameter: nameof(ProviderService.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: providerService.UpdatedDate,
                    secondDate: providerService.CreatedDate,
                    secondDateName: nameof(ProviderService.CreatedDate)),
                    Parameter: nameof(ProviderService.UpdatedDate))
            );
        }

        public void ValidateAgainstStorageProviderServiceOnModify(ProviderService inputProviderService, ProviderService storageProviderService)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputProviderService.CreatedDate,
                    secondDate: storageProviderService.CreatedDate,
                    secondDateName: nameof(ProviderService.CreatedDate)),
                    Parameter: nameof(ProviderService.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputProviderService.UpdatedDate,
                    secondDate: storageProviderService.UpdatedDate,
                    secondDateName: nameof(ProviderService.UpdatedDate)),
                    Parameter: nameof(ProviderService.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: inputProviderService.CreatedBy,
                    secondId: storageProviderService.CreatedBy,
                    secondIdName: nameof(ProviderService.CreatedBy)),
                    Parameter: nameof(ProviderService.CreatedBy))
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

        private static void ValidateProviderService(ProviderService providerService)
        {
            if (providerService is null)
            {
                throw new NullProviderServiceException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidProviderServiceException = new InvalidProviderServiceException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidProviderServiceException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidProviderServiceException.ThrowIfContainsErrors();
        }
    }
}