// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategory;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.ProvidersDirectors
{
    public partial class ProvidersDirectorService
    {
        private void ValidateProvidersDirectorOnRegister(ProvidersDirector provider)
        {
            ValidateProvidersDirector(provider);

            Validate(
                (Rule: IsInvalid(provider.Id), Parameter: nameof(ProvidersDirector.Id)),
                (Rule: IsInvalid(provider.Address), Parameter: nameof(ProvidersDirector.Address)),
                (Rule: IsInvalid(provider.ProviderId), Parameter: nameof(ProvidersDirector.ProviderId)),
                (Rule: IsInvalid(provider.ContactNumber), Parameter: nameof(ProvidersDirector.ContactNumber)),
                (Rule: IsInvalid(provider.Description), Parameter: nameof(ProvidersDirector.Description)),
                (Rule: IsInvalid(provider.Position), Parameter: nameof(ProvidersDirector.Position)),
                (Rule: IsInvalid(provider.FirstName), Parameter: nameof(ProvidersDirector.FirstName)),
                (Rule: IsInvalid(provider.LastName), Parameter: nameof(ProvidersDirector.LastName)),
                (Rule: IsInvalid(provider.MiddleName), Parameter: nameof(ProvidersDirector.MiddleName)),
                (Rule: IsInvalid(provider.CreatedBy), Parameter: nameof(ProvidersDirector.CreatedBy)),
                (Rule: IsInvalid(provider.UpdatedBy), Parameter: nameof(ProvidersDirector.UpdatedBy)),
                (Rule: IsInvalid(provider.CreatedDate), Parameter: nameof(ProvidersDirector.CreatedDate)),
                (Rule: IsInvalid(provider.UpdatedDate), Parameter: nameof(ProvidersDirector.UpdatedDate)),
                (Rule: IsNotRecent(provider.CreatedDate), Parameter: nameof(ProvidersDirector.CreatedDate)),

                (Rule: IsNotSame(firstId: provider.UpdatedBy,
                    secondId: provider.CreatedBy,
                    secondIdName: nameof(ProvidersDirector.CreatedBy)),
                    Parameter: nameof(ProvidersDirector.UpdatedBy)),

                (Rule: IsNotSame(firstDate: provider.UpdatedDate,
                    secondDate: provider.CreatedDate,
                    secondDateName: nameof(ProvidersDirector.CreatedDate)),
                    Parameter: nameof(ProvidersDirector.UpdatedDate))
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

        private static void ValidateProvidersDirectorId(Guid providerId)
        {
            Validate((Rule: IsInvalid(providerId), Parameter: nameof(ProvidersDirector.Id)));
        }

        private static void ValidateStorageProvidersDirector(ProvidersDirector storageProvidersDirector, Guid providerId)
        {
            if (storageProvidersDirector is null)
            {
                throw new NotFoundProvidersDirectorException(providerId);
            }
        }

        private void ValidateProvidersDirectorOnModify(ProvidersDirector provider)
        {
            ValidateProvidersDirector(provider);

            Validate(
                (Rule: IsInvalid(provider.Id), Parameter: nameof(ProvidersDirector.Id)),
                (Rule: IsInvalid(provider.Address), Parameter: nameof(ProvidersDirector.Address)),
                (Rule: IsInvalid(provider.ProviderId), Parameter: nameof(ProvidersDirector.ProviderId)),
                (Rule: IsInvalid(provider.ContactNumber), Parameter: nameof(ProvidersDirector.ContactNumber)),
                (Rule: IsInvalid(provider.Description), Parameter: nameof(ProvidersDirector.Description)),
                (Rule: IsInvalid(provider.Position), Parameter: nameof(ProvidersDirector.Position)),
                (Rule: IsInvalid(provider.FirstName), Parameter: nameof(ProvidersDirector.FirstName)),
                (Rule: IsInvalid(provider.LastName), Parameter: nameof(ProvidersDirector.LastName)),
                (Rule: IsInvalid(provider.MiddleName), Parameter: nameof(ProvidersDirector.MiddleName)),
                (Rule: IsInvalid(provider.CreatedBy), Parameter: nameof(ProvidersDirector.CreatedBy)),
                (Rule: IsInvalid(provider.UpdatedBy), Parameter: nameof(ProvidersDirector.UpdatedBy)),
                (Rule: IsInvalid(provider.CreatedDate), Parameter: nameof(ProvidersDirector.CreatedDate)),
                (Rule: IsInvalid(provider.UpdatedDate), Parameter: nameof(ProvidersDirector.UpdatedDate)),
                (Rule: IsNotRecent(provider.UpdatedDate), Parameter: nameof(ProvidersDirector.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: provider.UpdatedDate,
                    secondDate: provider.CreatedDate,
                    secondDateName: nameof(ProvidersDirector.CreatedDate)),
                    Parameter: nameof(ProvidersDirector.UpdatedDate))
            );
        }

        public void ValidateAgainstStorageProvidersDirectorOnModify(ProvidersDirector inputProvidersDirector, ProvidersDirector storageProvidersDirector)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputProvidersDirector.CreatedDate,
                    secondDate: storageProvidersDirector.CreatedDate,
                    secondDateName: nameof(ProvidersDirector.CreatedDate)),
                    Parameter: nameof(ProvidersDirector.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputProvidersDirector.UpdatedDate,
                    secondDate: storageProvidersDirector.UpdatedDate,
                    secondDateName: nameof(ProvidersDirector.UpdatedDate)),
                    Parameter: nameof(ProvidersDirector.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: inputProvidersDirector.CreatedBy,
                    secondId: storageProvidersDirector.CreatedBy,
                    secondIdName: nameof(ProvidersDirector.CreatedBy)),
                    Parameter: nameof(ProvidersDirector.CreatedBy))
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

        private static void ValidateProvidersDirector(ProvidersDirector provider)
        {
            if (provider is null)
            {
                throw new NullProvidersDirectorException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidProvidersDirectorException = new InvalidProvidersDirectorException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidProvidersDirectorException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidProvidersDirectorException.ThrowIfContainsErrors();
        }
    }
}