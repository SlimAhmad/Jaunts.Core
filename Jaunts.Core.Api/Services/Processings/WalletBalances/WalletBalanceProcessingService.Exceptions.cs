using Jaunts.Core.Api.Models.Processings.WalletBalance.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances.Exceptions;
using Xeptions;

namespace Jaunts.Core.Api.Services.Processings.WalletBalances
{

    public partial class WalletBalanceProcessingService
    {
       

        private delegate ValueTask<bool> ReturningBooleanFunction();
        private delegate ValueTask<WalletBalance> ReturningWalletBalanceFunction();
        private delegate IQueryable<WalletBalance> ReturningQueryableWalletBalanceFunction();

        private async ValueTask<bool> TryCatch(ReturningBooleanFunction returningBooleanFunction)
        {
            try
            {
                return await returningBooleanFunction();
            }
            catch (NullWalletBalanceProcessingException nullWalletBalanceProcessingException)
            {
                throw CreateAndLogValidationException(nullWalletBalanceProcessingException);
            }
            catch (InvalidWalletBalanceProcessingException invalidWalletBalanceProcessingException)
            {
                throw CreateAndLogValidationException(invalidWalletBalanceProcessingException);
            }
            catch (WalletBalanceValidationException roleValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleValidationException);
            }
            catch (WalletBalanceDependencyValidationException roleDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleDependencyValidationException);
            }
            catch (WalletBalanceDependencyException roleDependencyException)
            {
                throw CreateAndLogDependencyException(roleDependencyException);
            }
            catch (WalletBalanceServiceException roleServiceException)
            {
                throw CreateAndLogDependencyException(roleServiceException);
            }
            catch (Exception exception)
            {
                var failedWalletBalanceProcessingServiceException =
                    new FailedWalletBalanceProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedWalletBalanceProcessingServiceException);
            }
        }
        private async ValueTask<WalletBalance> TryCatch(ReturningWalletBalanceFunction returningWalletBalanceFunction)
        {
            try
            {
                return await returningWalletBalanceFunction();
            }
            catch (NullWalletBalanceProcessingException nullWalletBalanceProcessingException)
            {
                throw CreateAndLogValidationException(nullWalletBalanceProcessingException);
            }
            catch (InvalidWalletBalanceProcessingException invalidWalletBalanceProcessingException)
            {
                throw CreateAndLogValidationException(invalidWalletBalanceProcessingException);
            }
            catch (WalletBalanceValidationException roleValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleValidationException);
            }
            catch (WalletBalanceDependencyValidationException roleDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleDependencyValidationException);
            }
            catch (WalletBalanceDependencyException roleDependencyException)
            {
                throw CreateAndLogDependencyException(roleDependencyException);
            }
            catch (WalletBalanceServiceException roleServiceException)
            {
                throw CreateAndLogDependencyException(roleServiceException);
            }
            catch (Exception exception)
            {
                var failedWalletBalanceProcessingServiceException =
                    new FailedWalletBalanceProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedWalletBalanceProcessingServiceException);
            }
        }
        private IQueryable<WalletBalance> TryCatch(ReturningQueryableWalletBalanceFunction returningQueryableWalletBalanceFunction)
        {
            try
            {
                return returningQueryableWalletBalanceFunction();
            }
            catch (NullWalletBalanceProcessingException nullWalletBalanceProcessingException)
            {
                throw CreateAndLogValidationException(nullWalletBalanceProcessingException);
            }
            catch (InvalidWalletBalanceProcessingException invalidWalletBalanceProcessingException)
            {
                throw CreateAndLogValidationException(invalidWalletBalanceProcessingException);
            }
            catch (WalletBalanceValidationException roleValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleValidationException);
            }
            catch (WalletBalanceDependencyValidationException roleDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleDependencyValidationException);
            }
            catch (WalletBalanceDependencyException roleDependencyException)
            {
                throw CreateAndLogDependencyException(roleDependencyException);
            }
            catch (WalletBalanceServiceException roleServiceException)
            {
                throw CreateAndLogDependencyException(roleServiceException);
            }
            catch (Exception exception)
            {
                var failedWalletBalanceProcessingServiceException =
                    new FailedWalletBalanceProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedWalletBalanceProcessingServiceException);
            }
        }


        private WalletBalanceProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var roleProcessingServiceException = new
                WalletBalanceProcessingServiceException(exception);

            this.loggingBroker.LogError(roleProcessingServiceException);

            return roleProcessingServiceException;
        }

        private WalletBalanceProcessingDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var roleProcessingDependencyValidationException =
                new WalletBalanceProcessingDependencyValidationException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(roleProcessingDependencyValidationException);

            return roleProcessingDependencyValidationException;
        }

        private WalletBalanceProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var roleProcessingDependencyException =
                new WalletBalanceProcessingDependencyException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(roleProcessingDependencyException);

            return roleProcessingDependencyException;
        }

        private WalletBalanceProcessingValidationException CreateAndLogValidationException(
            Xeption exception)
        {
            var roleProcessingValidationException =
                new WalletBalanceProcessingValidationException(exception);

            this.loggingBroker.LogError(roleProcessingValidationException);

            return roleProcessingValidationException;
        }
    }
}