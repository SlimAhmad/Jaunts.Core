// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.WalletBalances
{
    public partial class WalletBalanceService
    {
        private void ValidateWalletBalanceOnCreate(WalletBalance walletBalance)
        {
            ValidateWalletBalance(walletBalance);

            Validate(
                (Rule: IsInvalid(walletBalance.Id), Parameter: nameof(WalletBalance.Id)),
                (Rule: IsInvalid(walletBalance.WalletId), Parameter: nameof(WalletBalance.WalletId)),
                (Rule: IsInvalid(walletBalance.CreatedBy), Parameter: nameof(WalletBalance.CreatedBy)),
                (Rule: IsInvalid(walletBalance.UpdatedBy), Parameter: nameof(WalletBalance.UpdatedBy)),
                (Rule: IsInvalid(walletBalance.CreatedDate), Parameter: nameof(WalletBalance.CreatedDate)),
                (Rule: IsInvalid(walletBalance.UpdatedDate), Parameter: nameof(WalletBalance.UpdatedDate)),
                (Rule: IsNotRecent(walletBalance.CreatedDate), Parameter: nameof(WalletBalance.CreatedDate)),

                (Rule: IsNotSame(firstId: walletBalance.UpdatedBy,
                    secondId: walletBalance.CreatedBy,
                    secondIdName: nameof(WalletBalance.CreatedBy)),
                    Parameter: nameof(WalletBalance.UpdatedBy)),

                (Rule: IsNotSame(firstDate: walletBalance.UpdatedDate,
                    secondDate: walletBalance.CreatedDate,
                    secondDateName: nameof(WalletBalance.CreatedDate)),
                    Parameter: nameof(WalletBalance.UpdatedDate))
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


        private static void ValidateWalletBalanceId(Guid walletBalanceId)
        {
            Validate((Rule: IsInvalid(walletBalanceId), Parameter: nameof(WalletBalance.Id)));
        }

        private static void ValidateStorageWalletBalance(WalletBalance storageWalletBalance, Guid walletBalanceId)
        {
            if (storageWalletBalance is null)
            {
                throw new NotFoundWalletBalanceException(walletBalanceId);
            }
        }

        private void ValidateWalletBalanceOnModify(WalletBalance walletBalance)
        {
            ValidateWalletBalance(walletBalance);

            Validate(
                (Rule: IsInvalid(walletBalance.Id), Parameter: nameof(WalletBalance.Id)),
                (Rule: IsInvalid(walletBalance.WalletId), Parameter: nameof(WalletBalance.WalletId)),
                (Rule: IsInvalid(walletBalance.CreatedBy), Parameter: nameof(WalletBalance.CreatedBy)),
                (Rule: IsInvalid(walletBalance.UpdatedBy), Parameter: nameof(WalletBalance.UpdatedBy)),
                (Rule: IsInvalid(walletBalance.CreatedDate), Parameter: nameof(WalletBalance.CreatedDate)),
                (Rule: IsInvalid(walletBalance.UpdatedDate), Parameter: nameof(WalletBalance.UpdatedDate)),
                (Rule: IsNotRecent(walletBalance.UpdatedDate), Parameter: nameof(WalletBalance.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: walletBalance.UpdatedDate,
                    secondDate: walletBalance.CreatedDate,
                    secondDateName: nameof(WalletBalance.CreatedDate)),
                    Parameter: nameof(WalletBalance.UpdatedDate))
            );
        }

        public void ValidateAgainstStorageWalletBalanceOnModify(WalletBalance inputWalletBalance, WalletBalance storageWalletBalance)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputWalletBalance.CreatedDate,
                    secondDate: storageWalletBalance.CreatedDate,
                    secondDateName: nameof(WalletBalance.CreatedDate)),
                    Parameter: nameof(WalletBalance.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputWalletBalance.UpdatedDate,
                    secondDate: storageWalletBalance.UpdatedDate,
                    secondDateName: nameof(WalletBalance.UpdatedDate)),
                    Parameter: nameof(WalletBalance.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: inputWalletBalance.CreatedBy,
                    secondId: storageWalletBalance.CreatedBy,
                    secondIdName: nameof(WalletBalance.CreatedBy)),
                    Parameter: nameof(WalletBalance.CreatedBy))
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

        private static void ValidateWalletBalance(WalletBalance walletBalance)
        {
            if (walletBalance is null)
            {
                throw new NullWalletBalanceException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidWalletBalanceException = new InvalidWalletBalanceException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidWalletBalanceException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidWalletBalanceException.ThrowIfContainsErrors();
        }
    }
}