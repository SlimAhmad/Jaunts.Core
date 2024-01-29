// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Drivers;

namespace Jaunts.Core.Api.Services.Foundations.Drivers
{
    public interface IDriverService
    {
        ValueTask<Driver> CreateDriverAsync(Driver driver);
        IQueryable<Driver> RetrieveAllDrivers();
        ValueTask<Driver> RetrieveDriverByIdAsync(Guid driverId);
        ValueTask<Driver> ModifyDriverAsync(Driver driver);
        ValueTask<Driver> RemoveDriverByIdAsync(Guid driverId);
    }
}