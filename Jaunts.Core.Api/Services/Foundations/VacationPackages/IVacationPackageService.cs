// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.VacationPackages;

namespace Jaunts.Core.Api.Services.Foundations.VacationPackages
{
    public interface IVacationPackageService 
    {
        ValueTask<VacationPackage> RegisterVacationPackageAsync(VacationPackage vacationPackage);
        IQueryable<VacationPackage> RetrieveAllVacationPackages();
        ValueTask<VacationPackage> RetrieveVacationPackageByIdAsync(Guid vacationPackageId);
        ValueTask<VacationPackage> ModifyVacationPackageAsync(VacationPackage vacationPackage);
        ValueTask<VacationPackage> RemoveVacationPackageByIdAsync(Guid vacationPackageId);
    }
}