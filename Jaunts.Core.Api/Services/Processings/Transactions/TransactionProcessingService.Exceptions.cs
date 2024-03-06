using Jaunts.Core.Api.Models.Processings.Transaction.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Transactions;
using Jaunts.Core.Api.Models.Services.Foundations.Transactions.Exceptions;
using Xeptions;

namespace Jaunts.Core.Api.Services.Processings.Transactions
{

    public partial class TransactionProcessingService
    {
       

        private delegate ValueTask<bool> ReturningBooleanFunction();
        private delegate ValueTask<Transaction> ReturningTransactionFunction();
        private delegate IQueryable<Transaction> ReturningQueryableTransactionFunction();

        private async ValueTask<bool> TryCatch(ReturningBooleanFunction returningBooleanFunction)
        {
            try
            {
                return await returningBooleanFunction();
            }
            catch (NullTransactionProcessingException nullTransactionProcessingException)
            {
                throw CreateAndLogValidationException(nullTransactionProcessingException);
            }
            catch (InvalidTransactionProcessingException invalidTransactionProcessingException)
            {
                throw CreateAndLogValidationException(invalidTransactionProcessingException);
            }
            catch (TransactionValidationException roleValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleValidationException);
            }
            catch (TransactionDependencyValidationException roleDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleDependencyValidationException);
            }
            catch (TransactionDependencyException roleDependencyException)
            {
                throw CreateAndLogDependencyException(roleDependencyException);
            }
            catch (TransactionServiceException roleServiceException)
            {
                throw CreateAndLogDependencyException(roleServiceException);
            }
            catch (Exception exception)
            {
                var failedTransactionProcessingServiceException =
                    new FailedTransactionProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedTransactionProcessingServiceException);
            }
        }
        private async ValueTask<Transaction> TryCatch(ReturningTransactionFunction returningTransactionFunction)
        {
            try
            {
                return await returningTransactionFunction();
            }
            catch (NullTransactionProcessingException nullTransactionProcessingException)
            {
                throw CreateAndLogValidationException(nullTransactionProcessingException);
            }
            catch (InvalidTransactionProcessingException invalidTransactionProcessingException)
            {
                throw CreateAndLogValidationException(invalidTransactionProcessingException);
            }
            catch (TransactionValidationException roleValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleValidationException);
            }
            catch (TransactionDependencyValidationException roleDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleDependencyValidationException);
            }
            catch (TransactionDependencyException roleDependencyException)
            {
                throw CreateAndLogDependencyException(roleDependencyException);
            }
            catch (TransactionServiceException roleServiceException)
            {
                throw CreateAndLogDependencyException(roleServiceException);
            }
            catch (Exception exception)
            {
                var failedTransactionProcessingServiceException =
                    new FailedTransactionProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedTransactionProcessingServiceException);
            }
        }
        private IQueryable<Transaction> TryCatch(ReturningQueryableTransactionFunction returningQueryableTransactionFunction)
        {
            try
            {
                return returningQueryableTransactionFunction();
            }
            catch (NullTransactionProcessingException nullTransactionProcessingException)
            {
                throw CreateAndLogValidationException(nullTransactionProcessingException);
            }
            catch (InvalidTransactionProcessingException invalidTransactionProcessingException)
            {
                throw CreateAndLogValidationException(invalidTransactionProcessingException);
            }
            catch (TransactionValidationException roleValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleValidationException);
            }
            catch (TransactionDependencyValidationException roleDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleDependencyValidationException);
            }
            catch (TransactionDependencyException roleDependencyException)
            {
                throw CreateAndLogDependencyException(roleDependencyException);
            }
            catch (TransactionServiceException roleServiceException)
            {
                throw CreateAndLogDependencyException(roleServiceException);
            }
            catch (Exception exception)
            {
                var failedTransactionProcessingServiceException =
                    new FailedTransactionProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedTransactionProcessingServiceException);
            }
        }


        private TransactionProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var roleProcessingServiceException = new
                TransactionProcessingServiceException(exception);

            this.loggingBroker.LogError(roleProcessingServiceException);

            return roleProcessingServiceException;
        }

        private TransactionProcessingDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var roleProcessingDependencyValidationException =
                new TransactionProcessingDependencyValidationException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(roleProcessingDependencyValidationException);

            return roleProcessingDependencyValidationException;
        }

        private TransactionProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var roleProcessingDependencyException =
                new TransactionProcessingDependencyException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(roleProcessingDependencyException);

            return roleProcessingDependencyException;
        }

        private TransactionProcessingValidationException CreateAndLogValidationException(
            Xeption exception)
        {
            var roleProcessingValidationException =
                new TransactionProcessingValidationException(exception);

            this.loggingBroker.LogError(roleProcessingValidationException);

            return roleProcessingValidationException;
        }
    }
}