using Jaunts.Core.Api.Models.Services.Foundations.Packages;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Package> InsertPackageAsync(
            Package package);

        IQueryable<Package> SelectAllPackage();

        ValueTask<Package> SelectPackageByIdAsync(
            Guid packageId);

        ValueTask<Package> UpdatePackageAsync(
            Package package);

        ValueTask<Package> DeletePackageAsync(
            Package package);
    }
}
