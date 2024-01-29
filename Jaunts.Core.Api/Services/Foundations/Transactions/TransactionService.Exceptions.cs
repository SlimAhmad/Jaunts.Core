// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Transactions;
using Jaunts.Core.Api.Models.Services.Foundations.Transactions.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace Jaunts.Core.Api.Services.Foundations.Transactions
{
    public partial class TransactionService
    {
        private delegate ValueTask<Transaction> ReturningTransactionFunction();
        private delegate IQueryable<Transaction> ReturningTransactionsFunction();

        private async ValueTask<Transaction> TryCatch(ReturningTransactionFunction returningTransactionFunction)
        {
            try
            {
                return await returningTransactionFunction();
            }
            catch (NullTransactionException nullTransactionException)
            {
                throw CreateAndLogValidationException(nullTransactionException);
            }
            catch (InvalidTransactionException invalidTransactionException)
            {
                throw CreateAndLogValidationException(invalidTransactionException);
            }
            catch (NotFoundTransactionException nullTransactionException)
            {
                throw CreateAndLogValidationException(nullTransactionException);
            }
            catch (SqlException sqlException)
            {
                var failedTransactionStorageException =
                    new FailedTransactionStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedTransactionStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsTransactionException =
                    new AlreadyExistsTransactionException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsTransactionException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedTransactionException = new LockedTransactionException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedTransactionException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedTransactionStorageException =
                    new FailedTransactionStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedTransactionStorageException);
            }
            catch (Exception exception)
            {
                var failedTransactionServiceException =
                    new FailedTransactionServiceException(exception);

                throw CreateAndLogServiceException(failedTransactionServiceException);
            }
        }

        private IQueryable<Transaction> TryCatch(ReturningTransactionsFunction returningTransactionsFunction)
        {
            try
            {
                return returningTransactionsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedTransactionStorageException =
                     new FailedTransactionStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedTransactionStorageException);
            }
            catch (Exception exception)
            {
                var failedTransactionServiceException =
                    new FailedTransactionServiceException(exception);

                throw CreateAndLogServiceException(failedTransactionServiceException);
            }
        }

        private TransactionValidationException CreateAndLogValidationException(Xeption exception)
        {
            var studentValidationException = new TransactionValidationException(exception);
            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }
        private TransactionDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)

        {
            var studentDependencyValidationException =
                new TransactionDependencyValidationException(exception);

            this.loggingBroker.LogError(studentDependencyValidationException);

            return studentDependencyValidationException;
        }

        private TransactionDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var studentDependencyException = new TransactionDependencyException(exception);
            this.loggingBroker.LogError(studentDependencyException);

            return studentDependencyException;
        }

        private TransactionDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var studentDependencyException = new TransactionDependencyException(exception);
            this.loggingBroker.LogCritical(studentDependencyException);

            return studentDependencyException;
        }

        private TransactionServiceException CreateAndLogServiceException(Xeption exception)
        {
            var studentServiceException = new TransactionServiceException(exception);
            this.loggingBroker.LogError(studentServiceException);

            return studentServiceException;
        }
    }
}
