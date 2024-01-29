// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace Jaunts.Core.Api.Services.Foundations.WalletBalances
{
    public partial class WalletBalanceService
    {
        private delegate ValueTask<WalletBalance> ReturningWalletBalanceFunction();
        private delegate IQueryable<WalletBalance> ReturningWalletBalancesFunction();

        private async ValueTask<WalletBalance> TryCatch(ReturningWalletBalanceFunction returningWalletBalanceFunction)
        {
            try
            {
                return await returningWalletBalanceFunction();
            }
            catch (NullWalletBalanceException nullWalletBalanceException)
            {
                throw CreateAndLogValidationException(nullWalletBalanceException);
            }
            catch (InvalidWalletBalanceException invalidWalletBalanceException)
            {
                throw CreateAndLogValidationException(invalidWalletBalanceException);
            }
            catch (NotFoundWalletBalanceException nullWalletBalanceException)
            {
                throw CreateAndLogValidationException(nullWalletBalanceException);
            }
            catch (SqlException sqlException)
            {
                var failedWalletBalanceStorageException =
                    new FailedWalletBalanceStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedWalletBalanceStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsWalletBalanceException =
                    new AlreadyExistsWalletBalanceException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsWalletBalanceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedWalletBalanceException = new LockedWalletBalanceException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedWalletBalanceException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedWalletBalanceStorageException =
                    new FailedWalletBalanceStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedWalletBalanceStorageException);
            }
            catch (Exception exception)
            {
                var failedWalletBalanceServiceException =
                    new FailedWalletBalanceServiceException(exception);

                throw CreateAndLogServiceException(failedWalletBalanceServiceException);
            }
        }

        private IQueryable<WalletBalance> TryCatch(ReturningWalletBalancesFunction returningWalletBalancesFunction)
        {
            try
            {
                return returningWalletBalancesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedWalletBalanceStorageException =
                     new FailedWalletBalanceStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedWalletBalanceStorageException);
            }
            catch (Exception exception)
            {
                var failedWalletBalanceServiceException =
                    new FailedWalletBalanceServiceException(exception);

                throw CreateAndLogServiceException(failedWalletBalanceServiceException);
            }
        }

        private WalletBalanceValidationException CreateAndLogValidationException(Xeption exception)
        {
            var studentValidationException = new WalletBalanceValidationException(exception);
            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }
        private WalletBalanceDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)

        {
            var studentDependencyValidationException =
                new WalletBalanceDependencyValidationException(exception);

            this.loggingBroker.LogError(studentDependencyValidationException);

            return studentDependencyValidationException;
        }

        private WalletBalanceDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var studentDependencyException = new WalletBalanceDependencyException(exception);
            this.loggingBroker.LogError(studentDependencyException);

            return studentDependencyException;
        }

        private WalletBalanceDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var studentDependencyException = new WalletBalanceDependencyException(exception);
            this.loggingBroker.LogCritical(studentDependencyException);

            return studentDependencyException;
        }

        private WalletBalanceServiceException CreateAndLogServiceException(Xeption exception)
        {
            var studentServiceException = new WalletBalanceServiceException(exception);
            this.loggingBroker.LogError(studentServiceException);

            return studentServiceException;
        }
    }
}
