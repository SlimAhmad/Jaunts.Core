using Jaunts.Core.Api.Models.Processings.Wallet.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets.Exceptions;
using Xeptions;

namespace Jaunts.Core.Api.Services.Processings.Wallets
{

    public partial class WalletProcessingService
    {
       

        private delegate ValueTask<bool> ReturningBooleanFunction();
        private delegate ValueTask<Wallet> ReturningWalletFunction();
        private delegate IQueryable<Wallet> ReturningQueryableWalletFunction();

        private async ValueTask<bool> TryCatch(ReturningBooleanFunction returningBooleanFunction)
        {
            try
            {
                return await returningBooleanFunction();
            }
            catch (NullWalletProcessingException nullWalletProcessingException)
            {
                throw CreateAndLogValidationException(nullWalletProcessingException);
            }
            catch (InvalidWalletProcessingException invalidWalletProcessingException)
            {
                throw CreateAndLogValidationException(invalidWalletProcessingException);
            }
            catch (WalletValidationException roleValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleValidationException);
            }
            catch (WalletDependencyValidationException roleDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleDependencyValidationException);
            }
            catch (WalletDependencyException roleDependencyException)
            {
                throw CreateAndLogDependencyException(roleDependencyException);
            }
            catch (WalletServiceException roleServiceException)
            {
                throw CreateAndLogDependencyException(roleServiceException);
            }
            catch (Exception exception)
            {
                var failedWalletProcessingServiceException =
                    new FailedWalletProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedWalletProcessingServiceException);
            }
        }
        private async ValueTask<Wallet> TryCatch(ReturningWalletFunction returningWalletFunction)
        {
            try
            {
                return await returningWalletFunction();
            }
            catch (NullWalletProcessingException nullWalletProcessingException)
            {
                throw CreateAndLogValidationException(nullWalletProcessingException);
            }
            catch (InvalidWalletProcessingException invalidWalletProcessingException)
            {
                throw CreateAndLogValidationException(invalidWalletProcessingException);
            }
            catch (WalletValidationException roleValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleValidationException);
            }
            catch (WalletDependencyValidationException roleDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleDependencyValidationException);
            }
            catch (WalletDependencyException roleDependencyException)
            {
                throw CreateAndLogDependencyException(roleDependencyException);
            }
            catch (WalletServiceException roleServiceException)
            {
                throw CreateAndLogDependencyException(roleServiceException);
            }
            catch (Exception exception)
            {
                var failedWalletProcessingServiceException =
                    new FailedWalletProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedWalletProcessingServiceException);
            }
        }
        private IQueryable<Wallet> TryCatch(ReturningQueryableWalletFunction returningQueryableWalletFunction)
        {
            try
            {
                return returningQueryableWalletFunction();
            }
            catch (NullWalletProcessingException nullWalletProcessingException)
            {
                throw CreateAndLogValidationException(nullWalletProcessingException);
            }
            catch (InvalidWalletProcessingException invalidWalletProcessingException)
            {
                throw CreateAndLogValidationException(invalidWalletProcessingException);
            }
            catch (WalletValidationException roleValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleValidationException);
            }
            catch (WalletDependencyValidationException roleDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(roleDependencyValidationException);
            }
            catch (WalletDependencyException roleDependencyException)
            {
                throw CreateAndLogDependencyException(roleDependencyException);
            }
            catch (WalletServiceException roleServiceException)
            {
                throw CreateAndLogDependencyException(roleServiceException);
            }
            catch (Exception exception)
            {
                var failedWalletProcessingServiceException =
                    new FailedWalletProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedWalletProcessingServiceException);
            }
        }


        private WalletProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var roleProcessingServiceException = new
                WalletProcessingServiceException(exception);

            this.loggingBroker.LogError(roleProcessingServiceException);

            return roleProcessingServiceException;
        }

        private WalletProcessingDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var roleProcessingDependencyValidationException =
                new WalletProcessingDependencyValidationException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(roleProcessingDependencyValidationException);

            return roleProcessingDependencyValidationException;
        }

        private WalletProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var roleProcessingDependencyException =
                new WalletProcessingDependencyException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(roleProcessingDependencyException);

            return roleProcessingDependencyException;
        }

        private WalletProcessingValidationException CreateAndLogValidationException(
            Xeption exception)
        {
            var roleProcessingValidationException =
                new WalletProcessingValidationException(exception);

            this.loggingBroker.LogError(roleProcessingValidationException);

            return roleProcessingValidationException;
        }
    }
}