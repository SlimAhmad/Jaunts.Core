using Jaunts.Core.Api.Models.Processings.Transaction.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Transactions;
using Jaunts.Core.Api.Models.Services.Orchestration.Transaction.Exceptions;
using Xeptions;

namespace Jaunts.Core.Api.Services.Orchestration.Transactions
{
    public partial class TransactionOrchestrationService
    {

        private delegate ValueTask<int> ReturningPermissionsFunction();
        private delegate ValueTask<bool> ReturningBooleanFunction();
        private delegate ValueTask<Transaction> ReturningTransactionFunction();
        private delegate IQueryable<Transaction> ReturningQueryableTransactionFunction();

        private async ValueTask<int> TryCatch(ReturningPermissionsFunction returningPermissionsFunction)
        {
            try
            {
                return await returningPermissionsFunction();
            }
            catch (TransactionProcessingValidationException TransactionValidationException)
            {
                throw CreateAndLogDependencyValidationException(TransactionValidationException);
            }
            catch (TransactionProcessingDependencyValidationException TransactionDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(TransactionDependencyValidationException);
            }
            catch (TransactionProcessingDependencyException TransactionDependencyException)
            {
                throw CreateAndLogDependencyException(TransactionDependencyException);
            }
            catch (TransactionProcessingServiceException TransactionServiceException)
            {
                throw CreateAndLogDependencyException(TransactionServiceException);
            }
            catch (Exception exception)
            {
                var failedTransactionProcessingServiceException =
                    new FailedTransactionProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedTransactionProcessingServiceException);
            }
        }
        private async ValueTask<bool> TryCatch(ReturningBooleanFunction returningBooleanFunction)
        {
            try
            {
                return await returningBooleanFunction();
            }
            catch (TransactionProcessingValidationException TransactionValidationException)
            {
                throw CreateAndLogDependencyValidationException(TransactionValidationException);
            }
            catch (TransactionProcessingDependencyValidationException TransactionDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(TransactionDependencyValidationException);
            }
            catch (TransactionProcessingDependencyException TransactionDependencyException)
            {
                throw CreateAndLogDependencyException(TransactionDependencyException);
            }
            catch (TransactionProcessingServiceException TransactionServiceException)
            {
                throw CreateAndLogDependencyException(TransactionServiceException);
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
            catch (TransactionProcessingValidationException TransactionValidationException)
            {
                throw CreateAndLogDependencyValidationException(TransactionValidationException);
            }
            catch (TransactionProcessingDependencyValidationException TransactionDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(TransactionDependencyValidationException);
            }
            catch (TransactionProcessingDependencyException TransactionDependencyException)
            {
                throw CreateAndLogDependencyException(TransactionDependencyException);
            }
            catch (TransactionProcessingServiceException TransactionServiceException)
            {
                throw CreateAndLogDependencyException(TransactionServiceException);
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
            catch (TransactionProcessingValidationException TransactionValidationException)
            {
                throw CreateAndLogDependencyValidationException(TransactionValidationException);
            }
            catch (TransactionProcessingDependencyValidationException TransactionDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(TransactionDependencyValidationException);
            }
            catch (TransactionProcessingDependencyException TransactionDependencyException)
            {
                throw CreateAndLogDependencyException(TransactionDependencyException);
            }
            catch (TransactionProcessingServiceException TransactionServiceException)
            {
                throw CreateAndLogDependencyException(TransactionServiceException);
            }
            catch (Exception exception)
            {
                var failedTransactionProcessingServiceException =
                    new FailedTransactionProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedTransactionProcessingServiceException);
            }
        }


        private TransactionOrchestrationServiceException CreateAndLogServiceException(Xeption exception)
        {
            var TransactionOrchestrationServiceException = new
                TransactionOrchestrationServiceException(exception);

            this.loggingBroker.LogError(TransactionOrchestrationServiceException);

            return TransactionOrchestrationServiceException;
        }

        private TransactionOrchestrationDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var TransactionOrchestrationDependencyValidationException =
                new TransactionOrchestrationDependencyValidationException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(TransactionOrchestrationDependencyValidationException);

            return TransactionOrchestrationDependencyValidationException;
        }

        private TransactionOrchestrationDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var TransactionOrchestrationDependencyException =
                new TransactionOrchestrationDependencyException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(TransactionOrchestrationDependencyException);

            return TransactionOrchestrationDependencyException;
        }

      
    }
}
