// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Packages;

namespace Jaunts.Core.Api.Services.Foundations.Packages
{
    public interface IPackageService 
    {
        ValueTask<Package> CreatePackageAsync(Package Package);
        IQueryable<Package> RetrieveAllPackages();
        ValueTask<Package> RetrievePackageByIdAsync(Guid PackageId);
        ValueTask<Package> ModifyPackageAsync(Package Package);
        ValueTask<Package> RemovePackageByIdAsync(Guid PackageId);
    }
}