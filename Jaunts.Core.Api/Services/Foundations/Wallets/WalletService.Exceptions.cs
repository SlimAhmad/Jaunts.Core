// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace Jaunts.Core.Api.Services.Foundations.Wallets
{
    public partial class WalletService
    {
        private delegate ValueTask<Wallet> ReturningWalletFunction();
        private delegate IQueryable<Wallet> ReturningWalletsFunction();

        private async ValueTask<Wallet> TryCatch(ReturningWalletFunction returningWalletFunction)
        {
            try
            {
                return await returningWalletFunction();
            }
            catch (NullWalletException nullWalletException)
            {
                throw CreateAndLogValidationException(nullWalletException);
            }
            catch (InvalidWalletException invalidWalletException)
            {
                throw CreateAndLogValidationException(invalidWalletException);
            }
            catch (NotFoundWalletException nullWalletException)
            {
                throw CreateAndLogValidationException(nullWalletException);
            }
            catch (SqlException sqlException)
            {
                var failedWalletStorageException =
                    new FailedWalletStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedWalletStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsWalletException =
                    new AlreadyExistsWalletException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsWalletException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedWalletException = new LockedWalletException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedWalletException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedWalletStorageException =
                    new FailedWalletStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedWalletStorageException);
            }
            catch (Exception exception)
            {
                var failedWalletServiceException =
                    new FailedWalletServiceException(exception);

                throw CreateAndLogServiceException(failedWalletServiceException);
            }
        }

        private IQueryable<Wallet> TryCatch(ReturningWalletsFunction returningWalletsFunction)
        {
            try
            {
                return returningWalletsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedWalletStorageException =
                     new FailedWalletStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedWalletStorageException);
            }
            catch (Exception exception)
            {
                var failedWalletServiceException =
                    new FailedWalletServiceException(exception);

                throw CreateAndLogServiceException(failedWalletServiceException);
            }
        }

        private WalletValidationException CreateAndLogValidationException(Xeption exception)
        {
            var studentValidationException = new WalletValidationException(exception);
            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }
        private WalletDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)

        {
            var studentDependencyValidationException =
                new WalletDependencyValidationException(exception);

            this.loggingBroker.LogError(studentDependencyValidationException);

            return studentDependencyValidationException;
        }

        private WalletDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var studentDependencyException = new WalletDependencyException(exception);
            this.loggingBroker.LogError(studentDependencyException);

            return studentDependencyException;
        }

        private WalletDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var studentDependencyException = new WalletDependencyException(exception);
            this.loggingBroker.LogCritical(studentDependencyException);

            return studentDependencyException;
        }

        private WalletServiceException CreateAndLogServiceException(Xeption exception)
        {
            var studentServiceException = new WalletServiceException(exception);
            this.loggingBroker.LogError(studentServiceException);

            return studentServiceException;
        }
    }
}
