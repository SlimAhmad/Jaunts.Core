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
            catch (AccountAggregationDependencyException countryDependencyException)
            {
                throw CreateAndLogDependencyException(countryDependencyException);
            }
            catch (AccountAggregationServiceException countryServiceException)
            {
                throw CreateAndLogDependencyException(countryServiceException);
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
            catch (AccountAggregationDependencyException countryDependencyException)
            {
                throw CreateAndLogDependencyException(countryDependencyException);
            }
            catch (AccountAggregationServiceException countryServiceException)
            {
                throw CreateAndLogDependencyException(countryServiceException);
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
            var countryAggregationDependencyValidationException =
               new AccountAggregationDependencyValidationException(
                   exception.InnerException as Xeption);

            this.loggingBroker.LogError(countryAggregationDependencyValidationException);

            return countryAggregationDependencyValidationException;
        }
        private AccountAggregationDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var countryAggregationDependencyException =
              new AccountAggregationDependencyException(
                  exception.InnerException as Xeption);

            this.loggingBroker.LogError(countryAggregationDependencyException);

            return countryAggregationDependencyException;
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
