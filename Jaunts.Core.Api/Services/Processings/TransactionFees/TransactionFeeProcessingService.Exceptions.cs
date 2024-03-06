using Jaunts.Core.Api.Models.Processings.TransactionFee.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees;
using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees.Exceptions;
using Xeptions;

namespace Jaunts.Core.Api.Services.Processings.TransactionFees
{

    public partial class TransactionFeeProcessingService
    {
       

        private delegate ValueTask<bool> ReturningBooleanFunction();
        private delegate ValueTask<TransactionFee> ReturningTransactionFeeFunction();
        private delegate IQueryable<TransactionFee> ReturningQueryableTransactionFeeFunction();

        private async ValueTask<bool> TryCatch(ReturningBooleanFunction returningBooleanFunction)
        {
            try
            {
                return await returningBooleanFunction();
            }
            catch (NullTransactionFeeProcessingException nullTransactionFeeProcessingException)
            {
                throw CreateAndLogValidationException(nullTransactionFeeProcessingException);
            }
            catch (InvalidTransactionFeeProcessingException invalidTransactionFeeProcessingException)
            {
                throw CreateAndLogValidationException(invalidTransactionFeeProcessingException);
            }
            catch (TransactionFeeValidationException roleValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleValidationException);
            }
            catch (TransactionFeeDependencyValidationException roleDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleDependencyValidationException);
            }
            catch (TransactionFeeDependencyException roleDependencyException)
            {
                throw CreateAndLogDependencyException(roleDependencyException);
            }
            catch (TransactionFeeServiceException roleServiceException)
            {
                throw CreateAndLogDependencyException(roleServiceException);
            }
            catch (Exception exception)
            {
                var failedTransactionFeeProcessingServiceException =
                    new FailedTransactionFeeProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedTransactionFeeProcessingServiceException);
            }
        }
        private async ValueTask<TransactionFee> TryCatch(ReturningTransactionFeeFunction returningTransactionFeeFunction)
        {
            try
            {
                return await returningTransactionFeeFunction();
            }
            catch (NullTransactionFeeProcessingException nullTransactionFeeProcessingException)
            {
                throw CreateAndLogValidationException(nullTransactionFeeProcessingException);
            }
            catch (InvalidTransactionFeeProcessingException invalidTransactionFeeProcessingException)
            {
                throw CreateAndLogValidationException(invalidTransactionFeeProcessingException);
            }
            catch (TransactionFeeValidationException roleValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleValidationException);
            }
            catch (TransactionFeeDependencyValidationException roleDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleDependencyValidationException);
            }
            catch (TransactionFeeDependencyException roleDependencyException)
            {
                throw CreateAndLogDependencyException(roleDependencyException);
            }
            catch (TransactionFeeServiceException roleServiceException)
            {
                throw CreateAndLogDependencyException(roleServiceException);
            }
            catch (Exception exception)
            {
                var failedTransactionFeeProcessingServiceException =
                    new FailedTransactionFeeProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedTransactionFeeProcessingServiceException);
            }
        }
        private IQueryable<TransactionFee> TryCatch(ReturningQueryableTransactionFeeFunction returningQueryableTransactionFeeFunction)
        {
            try
            {
                return returningQueryableTransactionFeeFunction();
            }
            catch (NullTransactionFeeProcessingException nullTransactionFeeProcessingException)
            {
                throw CreateAndLogValidationException(nullTransactionFeeProcessingException);
            }
            catch (InvalidTransactionFeeProcessingException invalidTransactionFeeProcessingException)
            {
                throw CreateAndLogValidationException(invalidTransactionFeeProcessingException);
            }
            catch (TransactionFeeValidationException roleValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleValidationException);
            }
            catch (TransactionFeeDependencyValidationException roleDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleDependencyValidationException);
            }
            catch (TransactionFeeDependencyException roleDependencyException)
            {
                throw CreateAndLogDependencyException(roleDependencyException);
            }
            catch (TransactionFeeServiceException roleServiceException)
            {
                throw CreateAndLogDependencyException(roleServiceException);
            }
            catch (Exception exception)
            {
                var failedTransactionFeeProcessingServiceException =
                    new FailedTransactionFeeProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedTransactionFeeProcessingServiceException);
            }
        }


        private TransactionFeeProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var roleProcessingServiceException = new
                TransactionFeeProcessingServiceException(exception);

            this.loggingBroker.LogError(roleProcessingServiceException);

            return roleProcessingServiceException;
        }

        private TransactionFeeProcessingDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var roleProcessingDependencyValidationException =
                new TransactionFeeProcessingDependencyValidationException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(roleProcessingDependencyValidationException);

            return roleProcessingDependencyValidationException;
        }

        private TransactionFeeProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var roleProcessingDependencyException =
                new TransactionFeeProcessingDependencyException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(roleProcessingDependencyException);

            return roleProcessingDependencyException;
        }

        private TransactionFeeProcessingValidationException CreateAndLogValidationException(
            Xeption exception)
        {
            var roleProcessingValidationException =
                new TransactionFeeProcessingValidationException(exception);

            this.loggingBroker.LogError(roleProcessingValidationException);

            return roleProcessingValidationException;
        }
    }
}