// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Providers;
using Jaunts.Core.Api.Models.Services.Foundations.Providers.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.Providers
{
    public partial class ProviderService
    {
        private void ValidateProviderOnRegister(Provider provider)
        {
            ValidateProvider(provider);

            Validate(
                (Rule: IsInvalid(provider.Id), Parameter: nameof(Provider.Id)),
                (Rule: IsInvalid(provider.Address), Parameter: nameof(Provider.Address)),
                (Rule: IsInvalid(provider.CompanyName), Parameter: nameof(Provider.CompanyName)),
                (Rule: IsInvalid(provider.RcNumber), Parameter: nameof(Provider.RcNumber)),
                (Rule: IsInvalid(provider.Incorporation), Parameter: nameof(Provider.Incorporation)),
                (Rule: IsInvalid(provider.CreatedBy), Parameter: nameof(Provider.CreatedBy)),
                (Rule: IsInvalid(provider.UpdatedBy), Parameter: nameof(Provider.UpdatedBy)),
                (Rule: IsInvalid(provider.CreatedDate), Parameter: nameof(Provider.CreatedDate)),
                (Rule: IsInvalid(provider.UpdatedDate), Parameter: nameof(Provider.UpdatedDate)),
                (Rule: IsNotRecent(provider.CreatedDate), Parameter: nameof(Provider.CreatedDate)),

                (Rule: IsNotSame(firstId: provider.UpdatedBy,
                    secondId: provider.CreatedBy,
                    secondIdName: nameof(Provider.CreatedBy)),
                    Parameter: nameof(Provider.UpdatedBy)),

                (Rule: IsNotSame(firstDate: provider.UpdatedDate,
                    secondDate: provider.CreatedDate,
                    secondDateName: nameof(Provider.CreatedDate)),
                    Parameter: nameof(Provider.UpdatedDate))
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

        private static void ValidateProviderId(Guid providerId)
        {
            if (providerId == Guid.Empty)
            {
                throw new InvalidProviderException(
                    parameterName: nameof(Provider.Id),
                    parameterValue: providerId);
            }
        }

        private static void ValidateStorageProvider(Provider storageProvider, Guid providerId)
        {
            if (storageProvider is null)
            {
                throw new NotFoundProviderException(providerId);
            }
        }

        private void ValidateProviderOnModify(Provider provider)
        {
            ValidateProvider(provider);

            Validate(
                (Rule: IsInvalid(provider.Id), Parameter: nameof(Provider.Id)),
                (Rule: IsInvalid(provider.Address), Parameter: nameof(Provider.Address)),
                (Rule: IsInvalid(provider.CompanyName), Parameter: nameof(Provider.CompanyName)),
                (Rule: IsInvalid(provider.RcNumber), Parameter: nameof(Provider.RcNumber)),
                (Rule: IsInvalid(provider.Incorporation), Parameter: nameof(Provider.Incorporation)),
                (Rule: IsInvalid(provider.CreatedBy), Parameter: nameof(Provider.CreatedBy)),
                (Rule: IsInvalid(provider.UpdatedBy), Parameter: nameof(Provider.UpdatedBy)),
                (Rule: IsInvalid(provider.CreatedDate), Parameter: nameof(Provider.CreatedDate)),
                (Rule: IsInvalid(provider.UpdatedDate), Parameter: nameof(Provider.UpdatedDate)),
                (Rule: IsNotRecent(provider.UpdatedDate), Parameter: nameof(Provider.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: provider.UpdatedDate,
                    secondDate: provider.CreatedDate,
                    secondDateName: nameof(Provider.CreatedDate)),
                    Parameter: nameof(Provider.UpdatedDate))
            );
        }

        public void ValidateAgainstStorageProviderOnModify(Provider inputProvider, Provider storageProvider)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputProvider.CreatedDate,
                    secondDate: storageProvider.CreatedDate,
                    secondDateName: nameof(Provider.CreatedDate)),
                    Parameter: nameof(Provider.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputProvider.UpdatedDate,
                    secondDate: storageProvider.UpdatedDate,
                    secondDateName: nameof(Provider.UpdatedDate)),
                    Parameter: nameof(Provider.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: inputProvider.CreatedBy,
                    secondId: storageProvider.CreatedBy,
                    secondIdName: nameof(Provider.CreatedBy)),
                    Parameter: nameof(Provider.CreatedBy))
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

        private static void ValidateProvider(Provider provider)
        {
            if (provider is null)
            {
                throw new NullProviderException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidProviderException = new InvalidProviderException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidProviderException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidProviderException.ThrowIfContainsErrors();
        }
    }
}