// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Promos_Offers;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOffers.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace Jaunts.Core.Api.Services.Foundations.PromosOffers
{
    public partial class PromosOfferService
    {
        private delegate ValueTask<PromosOffer> ReturningPromosOfferFunction();
        private delegate IQueryable<PromosOffer> ReturningPromosOffersFunction();

        private async ValueTask<PromosOffer> TryCatch(ReturningPromosOfferFunction returningPromosOfferFunction)
        {
            try
            {
                return await returningPromosOfferFunction();
            }
            catch (NullPromosOffersException nullPromosOfferException)
            {
                throw CreateAndLogValidationException(nullPromosOfferException);
            }
            catch (InvalidPromosOffersException invalidPromosOfferException)
            {
                throw CreateAndLogValidationException(invalidPromosOfferException);
            }
            catch (NotFoundPromosOffersException nullPromosOfferException)
            {
                throw CreateAndLogValidationException(nullPromosOfferException);
            }
            catch (SqlException sqlException)
            {
                var failedPromosOfferStorageException =
                    new FailedPromosOffersStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedPromosOfferStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsPromosOfferException =
                    new AlreadyExistsPromosOffersException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsPromosOfferException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedPromosOfferException = new LockedPromosOffersException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedPromosOfferException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedPromosOfferStorageException =
                    new FailedPromosOffersStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedPromosOfferStorageException);
            }
            catch (Exception exception)
            {
                var failedPromosOfferServiceException =
                    new FailedPromosOffersServiceException(exception);

                throw CreateAndLogServiceException(failedPromosOfferServiceException);
            }
        }

        private IQueryable<PromosOffer> TryCatch(ReturningPromosOffersFunction returningPromosOffersFunction)
        {
            try
            {
                return returningPromosOffersFunction();
            }
            catch (SqlException sqlException)
            {
                var failedPromosOfferStorageException =
                     new FailedPromosOffersStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedPromosOfferStorageException);
            }
            catch (Exception exception)
            {
                var failedPromosOfferServiceException =
                    new FailedPromosOffersServiceException(exception);

                throw CreateAndLogServiceException(failedPromosOfferServiceException);
            }
        }

        private PromosOffersValidationException CreateAndLogValidationException(Xeption exception)
        {
            var studentValidationException = new PromosOffersValidationException(exception);
            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }
        private PromosOffersDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)

        {
            var studentDependencyValidationException =
                new PromosOffersDependencyValidationException(exception);

            this.loggingBroker.LogError(studentDependencyValidationException);

            return studentDependencyValidationException;
        }

        private PromosOffersDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var studentDependencyException = new PromosOffersDependencyException(exception);
            this.loggingBroker.LogError(studentDependencyException);

            return studentDependencyException;
        }

        private PromosOffersDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var studentDependencyException = new PromosOffersDependencyException(exception);
            this.loggingBroker.LogCritical(studentDependencyException);

            return studentDependencyException;
        }

        private PromosOffersServiceException CreateAndLogServiceException(Xeption exception)
        {
            var studentServiceException = new PromosOffersServiceException(exception);
            this.loggingBroker.LogError(studentServiceException);

            return studentServiceException;
        }
    }
}
