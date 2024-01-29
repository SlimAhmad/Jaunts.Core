// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals;

namespace Jaunts.Core.Api.Services.Foundations.FlightDeals
{
    public interface IFlightDealService
    {
        ValueTask<FlightDeal> CreateFlightDealAsync(FlightDeal fleet);
        IQueryable<FlightDeal> RetrieveAllFlightDeals();
        ValueTask<FlightDeal> RetrieveFlightDealByIdAsync(Guid fleetId);
        ValueTask<FlightDeal> ModifyFlightDealAsync(FlightDeal fleet);
        ValueTask<FlightDeal> RemoveFlightDealByIdAsync(Guid fleetId);
    }
}