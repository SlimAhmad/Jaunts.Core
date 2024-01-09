using Jaunts.Core.Api.Models.Services.Foundations.Rides;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Ride> InsertRideAsync(
            Ride ride);

        IQueryable<Ride> SelectAllRides();

        ValueTask<Ride> SelectRideByIdAsync(
            Guid rideId);

        ValueTask<Ride> UpdateRideAsync(
            Ride ride);

        ValueTask<Ride> DeleteRideAsync(
            Ride ride);
    }
}
