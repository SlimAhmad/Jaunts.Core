using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Aggregation.Account.Exceptions;
using Xeptions;

namespace Jaunts.Core.Api.Services.Aggregations.Account
{
    public partial class AccountAggregationService
    {
        private delegate ValueTask<UserAccountDetailsApiResponse> ReturningAccountFunction();
        private delegate ValueTask<Boolean> ReturningBooleanFunction();
        private async ValueTask<UserAccountDetailsApiResponse> TryCatch(
            ReturningAccountFunction returningAccountFunction)
        {
            try
            {
                return await returningAccountFunction();
            }
            catch (AccountAggregationDependencyException accountDependencyException)
            {
                throw CreateAndLogDependencyException(accountDependencyException);
            }
            catch (AccountAggregationServiceException accountServiceException)
            {
                throw CreateAndLogDependencyException(accountServiceException);
            }
            catch (Exception exception)
            {
                var failedAccountAggregationProcessingServiceException =
                    new FailedAccountAggregationServiceException(exception);

                throw CreateAndLogServiceException(failedAccountAggregationProcessingServiceException);
            }
        }
        private async ValueTask<bool> TryCatch(
        ReturningBooleanFunction returningBooleanFunction)
        {
            try
            {
                return await returningBooleanFunction();
            }
            catch (AccountAggregationDependencyException accountDependencyException)
            {
                throw CreateAndLogDependencyException(accountDependencyException);
            }
            catch (AccountAggregationServiceException accountServiceException)
            {
                throw CreateAndLogDependencyException(accountServiceException);
            }
            catch (Exception exception)
            {
                var failedAccountAggregationProcessingServiceException =
                    new FailedAccountAggregationServiceException(exception);

                throw CreateAndLogServiceException(failedAccountAggregationProcessingServiceException);
            }
        }

        private AccountAggregationDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var accountAggregationDependencyValidationException =
               new AccountAggregationDependencyValidationException(
                   exception.InnerException as Xeption);

            this.loggingBroker.LogError(accountAggregationDependencyValidationException);

            return accountAggregationDependencyValidationException;
        }
        private AccountAggregationDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var accountAggregationDependencyException =
              new AccountAggregationDependencyException(
                  exception.InnerException as Xeption);

            this.loggingBroker.LogError(accountAggregationDependencyException);

            return accountAggregationDependencyException;
        }

        private AccountAggregationServiceException CreateAndLogServiceException(Xeption exception)
        {
            var externalAccountAggregationServiceException =
               new AccountAggregationServiceException(exception);

            this.loggingBroker.LogError(externalAccountAggregationServiceException);

            return externalAccountAggregationServiceException;
        }
    }
}
