// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Wallets;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.Wallets
{
    public partial class WalletService
    {
        private void ValidateWalletOnCreate(Wallet wallet)
        {
            ValidateWallet(wallet);

            Validate(
                (Rule: IsInvalid(wallet.Id), Parameter: nameof(Wallet.Id)),
                (Rule: IsInvalid(wallet.UserId), Parameter: nameof(Wallet.UserId)),
                (Rule: IsInvalid(wallet.Status), Parameter: nameof(Wallet.Status)),
                (Rule: IsInvalid(wallet.Description), Parameter: nameof(Wallet.Description)),
                (Rule: IsInvalid(wallet.WalletName), Parameter: nameof(Wallet.WalletName)),
                (Rule: IsInvalid(wallet.CreatedBy), Parameter: nameof(Wallet.CreatedBy)),
                (Rule: IsInvalid(wallet.UpdatedBy), Parameter: nameof(Wallet.UpdatedBy)),
                (Rule: IsInvalid(wallet.CreatedDate), Parameter: nameof(Wallet.CreatedDate)),
                (Rule: IsInvalid(wallet.UpdatedDate), Parameter: nameof(Wallet.UpdatedDate)),
                (Rule: IsNotRecent(wallet.CreatedDate), Parameter: nameof(Wallet.CreatedDate)),

                (Rule: IsNotSame(firstId: wallet.UpdatedBy,
                    secondId: wallet.CreatedBy,
                    secondIdName: nameof(Wallet.CreatedBy)),
                    Parameter: nameof(Wallet.UpdatedBy)),

                (Rule: IsNotSame(firstDate: wallet.UpdatedDate,
                    secondDate: wallet.CreatedDate,
                    secondDateName: nameof(Wallet.CreatedDate)),
                    Parameter: nameof(Wallet.UpdatedDate))
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

        private static dynamic IsInvalid(WalletStatus status) => new
        {
            Condition = Enum.IsDefined(status) is false,
            Message = "Value is not recognized"
        };

        private static void ValidateWalletId(Guid walletId)
        {
            Validate((Rule: IsInvalid(walletId), Parameter: nameof(Wallet.Id)));
        }

        private static void ValidateStorageWallet(Wallet storageWallet, Guid walletId)
        {
            if (storageWallet is null)
            {
                throw new NotFoundWalletException(walletId);
            }
        }

        private void ValidateWalletOnModify(Wallet wallet)
        {
            ValidateWallet(wallet);

            Validate(
                (Rule: IsInvalid(wallet.Id), Parameter: nameof(Wallet.Id)),
                (Rule: IsInvalid(wallet.UserId), Parameter: nameof(Wallet.UserId)),
                (Rule: IsInvalid(wallet.Status), Parameter: nameof(Wallet.Status)),
                (Rule: IsInvalid(wallet.Description), Parameter: nameof(Wallet.Description)),
                (Rule: IsInvalid(wallet.WalletName), Parameter: nameof(Wallet.WalletName)),
                (Rule: IsInvalid(wallet.CreatedBy), Parameter: nameof(Wallet.CreatedBy)),
                (Rule: IsInvalid(wallet.UpdatedBy), Parameter: nameof(Wallet.UpdatedBy)),
                (Rule: IsInvalid(wallet.CreatedDate), Parameter: nameof(Wallet.CreatedDate)),
                (Rule: IsInvalid(wallet.UpdatedDate), Parameter: nameof(Wallet.UpdatedDate)),
                (Rule: IsNotRecent(wallet.UpdatedDate), Parameter: nameof(Wallet.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: wallet.UpdatedDate,
                    secondDate: wallet.CreatedDate,
                    secondDateName: nameof(Wallet.CreatedDate)),
                    Parameter: nameof(Wallet.UpdatedDate))
            );
        }

        public void ValidateAgainstStorageWalletOnModify(Wallet inputWallet, Wallet storageWallet)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputWallet.CreatedDate,
                    secondDate: storageWallet.CreatedDate,
                    secondDateName: nameof(Wallet.CreatedDate)),
                    Parameter: nameof(Wallet.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputWallet.UpdatedDate,
                    secondDate: storageWallet.UpdatedDate,
                    secondDateName: nameof(Wallet.UpdatedDate)),
                    Parameter: nameof(Wallet.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: inputWallet.CreatedBy,
                    secondId: storageWallet.CreatedBy,
                    secondIdName: nameof(Wallet.CreatedBy)),
                    Parameter: nameof(Wallet.CreatedBy))
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

        private static void ValidateWallet(Wallet wallet)
        {
            if (wallet is null)
            {
                throw new NullWalletException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidWalletException = new InvalidWalletException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidWalletException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidWalletException.ThrowIfContainsErrors();
        }
    }
}