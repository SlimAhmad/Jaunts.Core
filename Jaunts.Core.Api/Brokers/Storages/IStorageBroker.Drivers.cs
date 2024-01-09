using Jaunts.Core.Api.Models.Services.Foundations.Drivers;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Driver> InsertDriverAsync(
            Driver driver);

        IQueryable<Driver> SelectAllDrivers();

        ValueTask<Driver> SelectDriverByIdAsync(
            Guid driverId);

        ValueTask<Driver> UpdateDriverAsync(
            Driver driver);

        ValueTask<Driver> DeleteDriverAsync(
            Driver driver);
    }
}
