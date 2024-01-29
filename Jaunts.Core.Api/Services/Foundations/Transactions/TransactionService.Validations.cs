// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Transactions;
using Jaunts.Core.Api.Models.Services.Foundations.Transactions.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.Transactions
{
    public partial class TransactionService
    {
        private void ValidateTransactionOnRegister(Transaction transaction)
        {
            ValidateTransaction(transaction);

            Validate(
                (Rule: IsInvalid(transaction.Id), Parameter: nameof(Transaction.Id)),
                (Rule: IsInvalid(transaction.UserId), Parameter: nameof(Transaction.UserId)),
                (Rule: IsInvalid(transaction.WalletBalanceId), Parameter: nameof(Transaction.WalletBalanceId)),
                (Rule: IsInvalid(transaction.Narration), Parameter: nameof(Transaction.Narration)),
                (Rule: IsInvalid(transaction.TransactionType), Parameter: nameof(Transaction.TransactionType)),
                (Rule: IsInvalid(transaction.TransactionStatus), Parameter: nameof(Transaction.TransactionStatus)),
                (Rule: IsInvalid(transaction.CreatedBy), Parameter: nameof(Transaction.CreatedBy)),
                (Rule: IsInvalid(transaction.UpdatedBy), Parameter: nameof(Transaction.UpdatedBy)),
                (Rule: IsInvalid(transaction.CreatedDate), Parameter: nameof(Transaction.CreatedDate)),
                (Rule: IsInvalid(transaction.UpdatedDate), Parameter: nameof(Transaction.UpdatedDate)),
                (Rule: IsNotRecent(transaction.CreatedDate), Parameter: nameof(Transaction.CreatedDate)),

                (Rule: IsNotSame(firstId: transaction.UpdatedBy,
                    secondId: transaction.CreatedBy,
                    secondIdName: nameof(Transaction.CreatedBy)),
                    Parameter: nameof(Transaction.UpdatedBy)),

                (Rule: IsNotSame(firstDate: transaction.UpdatedDate,
                    secondDate: transaction.CreatedDate,
                    secondDateName: nameof(Transaction.CreatedDate)),
                    Parameter: nameof(Transaction.UpdatedDate))
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

        private static dynamic IsInvalid(TransactionStatus status) => new
        {
            Condition = Enum.IsDefined(status) is false,
            Message = "Value is not recognized"
        };

        private static dynamic IsInvalid(TransactionType status) => new
        {
            Condition = Enum.IsDefined(status) is false,
            Message = "Value is not recognized"
        };

        private static void ValidateTransactionId(Guid transactionId)
        {
            Validate((Rule: IsInvalid(transactionId), Parameter: nameof(Transaction.Id)));
        }

        private static void ValidateStorageTransaction(Transaction storageTransaction, Guid transactionId)
        {
            if (storageTransaction is null)
            {
                throw new NotFoundTransactionException(transactionId);
            }
        }

        private void ValidateTransactionOnModify(Transaction transaction)
        {
            ValidateTransaction(transaction);

            Validate(
                (Rule: IsInvalid(transaction.Id), Parameter: nameof(Transaction.Id)),
                (Rule: IsInvalid(transaction.UserId), Parameter: nameof(Transaction.UserId)),
                (Rule: IsInvalid(transaction.WalletBalanceId), Parameter: nameof(Transaction.WalletBalanceId)),
                (Rule: IsInvalid(transaction.Narration), Parameter: nameof(Transaction.Narration)),
                (Rule: IsInvalid(transaction.TransactionType), Parameter: nameof(Transaction.TransactionType)),
                (Rule: IsInvalid(transaction.TransactionStatus), Parameter: nameof(Transaction.TransactionStatus)),
                (Rule: IsInvalid(transaction.CreatedBy), Parameter: nameof(Transaction.CreatedBy)),
                (Rule: IsInvalid(transaction.UpdatedBy), Parameter: nameof(Transaction.UpdatedBy)),
                (Rule: IsInvalid(transaction.CreatedDate), Parameter: nameof(Transaction.CreatedDate)),
                (Rule: IsInvalid(transaction.UpdatedDate), Parameter: nameof(Transaction.UpdatedDate)),
                (Rule: IsNotRecent(transaction.UpdatedDate), Parameter: nameof(Transaction.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: transaction.UpdatedDate,
                    secondDate: transaction.CreatedDate,
                    secondDateName: nameof(Transaction.CreatedDate)),
                    Parameter: nameof(Transaction.UpdatedDate))
            );
        }

        public void ValidateAgainstStorageTransactionOnModify(Transaction inputTransaction, Transaction storageTransaction)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputTransaction.CreatedDate,
                    secondDate: storageTransaction.CreatedDate,
                    secondDateName: nameof(Transaction.CreatedDate)),
                    Parameter: nameof(Transaction.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputTransaction.UpdatedDate,
                    secondDate: storageTransaction.UpdatedDate,
                    secondDateName: nameof(Transaction.UpdatedDate)),
                    Parameter: nameof(Transaction.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: inputTransaction.CreatedBy,
                    secondId: storageTransaction.CreatedBy,
                    secondIdName: nameof(Transaction.CreatedBy)),
                    Parameter: nameof(Transaction.CreatedBy))
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

        private static void ValidateTransaction(Transaction transaction)
        {
            if (transaction is null)
            {
                throw new NullTransactionException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidTransactionException = new InvalidTransactionException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidTransactionException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidTransactionException.ThrowIfContainsErrors();
        }
    }
}