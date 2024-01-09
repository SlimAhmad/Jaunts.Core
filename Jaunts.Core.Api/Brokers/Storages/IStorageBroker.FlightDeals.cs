using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<FlightDeal> InsertFlightDealAsync(
            FlightDeal flightDeals);

        IQueryable<FlightDeal> SelectAllFlightDeals();

        ValueTask<FlightDeal> SelectFlightDealByIdAsync(
            Guid customerId);

        ValueTask<FlightDeal> UpdateFlightDealAsync(
            FlightDeal flightDeals);

        ValueTask<FlightDeal> DeleteFlightDealAsync(
            FlightDeal flightDeals);
    }
}
