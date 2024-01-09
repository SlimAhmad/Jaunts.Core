// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees;
using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace Jaunts.Core.Api.Services.Foundations.TransactionFees
{
    public partial class TransactionFeeService
    {
        private delegate ValueTask<TransactionFee> ReturningTransactionFeeFunction();
        private delegate IQueryable<TransactionFee> ReturningTransactionFeesFunction();

        private async ValueTask<TransactionFee> TryCatch(ReturningTransactionFeeFunction returningTransactionFeeFunction)
        {
            try
            {
                return await returningTransactionFeeFunction();
            }
            catch (NullTransactionFeeException nullTransactionFeeException)
            {
                throw CreateAndLogValidationException(nullTransactionFeeException);
            }
            catch (InvalidTransactionFeeException invalidTransactionFeeException)
            {
                throw CreateAndLogValidationException(invalidTransactionFeeException);
            }
            catch (NotFoundTransactionFeeException nullTransactionFeeException)
            {
                throw CreateAndLogValidationException(nullTransactionFeeException);
            }
            catch (SqlException sqlException)
            {
                var failedTransactionFeeStorageException =
                    new FailedTransactionFeeStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedTransactionFeeStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsTransactionFeeException =
                    new AlreadyExistsTransactionFeeException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsTransactionFeeException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedTransactionFeeException = new LockedTransactionFeeException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedTransactionFeeException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedTransactionFeeStorageException =
                    new FailedTransactionFeeStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedTransactionFeeStorageException);
            }
            catch (Exception exception)
            {
                var failedTransactionFeeServiceException =
                    new FailedTransactionFeeServiceException(exception);

                throw CreateAndLogServiceException(failedTransactionFeeServiceException);
            }
        }

        private IQueryable<TransactionFee> TryCatch(ReturningTransactionFeesFunction returningTransactionFeesFunction)
        {
            try
            {
                return returningTransactionFeesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedTransactionFeeStorageException =
                     new FailedTransactionFeeStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedTransactionFeeStorageException);
            }
            catch (Exception exception)
            {
                var failedTransactionFeeServiceException =
                    new FailedTransactionFeeServiceException(exception);

                throw CreateAndLogServiceException(failedTransactionFeeServiceException);
            }
        }

        private TransactionFeeValidationException CreateAndLogValidationException(Xeption exception)
        {
            var studentValidationException = new TransactionFeeValidationException(exception);
            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }
        private TransactionFeeDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)

        {
            var studentDependencyValidationException =
                new TransactionFeeDependencyValidationException(exception);

            this.loggingBroker.LogError(studentDependencyValidationException);

            return studentDependencyValidationException;
        }

        private TransactionFeeDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var studentDependencyException = new TransactionFeeDependencyException(exception);
            this.loggingBroker.LogError(studentDependencyException);

            return studentDependencyException;
        }

        private TransactionFeeDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var studentDependencyException = new TransactionFeeDependencyException(exception);
            this.loggingBroker.LogCritical(studentDependencyException);

            return studentDependencyException;
        }

        private TransactionFeeServiceException CreateAndLogServiceException(Xeption exception)
        {
            var studentServiceException = new TransactionFeeServiceException(exception);
            this.loggingBroker.LogError(studentServiceException);

            return studentServiceException;
        }
    }
}
