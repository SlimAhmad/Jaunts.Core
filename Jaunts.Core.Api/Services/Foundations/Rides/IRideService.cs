// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Rides;

namespace Jaunts.Core.Api.Services.Foundations.Rides
{
    public interface IRideService
    {
        ValueTask<Ride> RegisterRideAsync(Ride ride);
        IQueryable<Ride> RetrieveAllRides();
        ValueTask<Ride> RetrieveRideByIdAsync(Guid rideId);
        ValueTask<Ride> ModifyRideAsync(Ride ride);
        ValueTask<Ride> RemoveRideByIdAsync(Guid rideId);
    }
}