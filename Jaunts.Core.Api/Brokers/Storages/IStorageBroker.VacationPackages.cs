using Jaunts.Core.Api.Models.Services.Foundations.VacationPackages;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<VacationPackage> InsertVacationPackageAsync(
            VacationPackage package);

        IQueryable<VacationPackage> SelectAllVacationPackages();

        ValueTask<VacationPackage> SelectVacationPackageByIdAsync(
            Guid packageId);

        ValueTask<VacationPackage> UpdateVacationPackageAsync(
            VacationPackage package);

        ValueTask<VacationPackage> DeleteVacationPackageAsync(
            VacationPackage package);
    }
}
