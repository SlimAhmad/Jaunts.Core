using Jaunts.Core.Api.Models.Services.Foundations.Fleets;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Fleet> InsertFleetAsync(
            Fleet fleet);

        IQueryable<Fleet> SelectAllFleets();

        ValueTask<Fleet> SelectFleetByIdAsync(
            Guid customerId);

        ValueTask<Fleet> UpdateFleetAsync(
            Fleet fleet);

        ValueTask<Fleet> DeleteFleetAsync(
            Fleet fleet);
    }
}
