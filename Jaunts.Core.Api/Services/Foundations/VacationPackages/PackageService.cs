// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.Packages;

namespace Jaunts.Core.Api.Services.Foundations.Packages
{
    public partial class PackageService : IPackageService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public PackageService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Package> CreatePackageAsync(Package package) =>
        TryCatch(async () =>
        {
            ValidatePackageOnRegister(package);

            return await this.storageBroker.InsertPackageAsync(package);
        });

        public IQueryable<Package> RetrieveAllPackages() =>
        TryCatch(() => this.storageBroker.SelectAllPackage());

        public ValueTask<Package> RetrievePackageByIdAsync(Guid PackageId) =>
        TryCatch(async () =>
        {
            ValidatePackageId(PackageId);
            Package maybePackage = await this.storageBroker.SelectPackageByIdAsync(PackageId);
            ValidateStoragePackage(maybePackage, PackageId);

            return maybePackage;
        });

        public ValueTask<Package> ModifyPackageAsync(Package Package) =>
        TryCatch(async () =>
        {
            ValidatePackageOnModify(Package);

            Package maybePackage =
                await this.storageBroker.SelectPackageByIdAsync(Package.Id);

            ValidateStoragePackage(maybePackage, Package.Id);
            ValidateAgainstStoragePackageOnModify(inputPackage: Package, storagePackage: maybePackage);

            return await this.storageBroker.UpdatePackageAsync(Package);
        });

        public ValueTask<Package> RemovePackageByIdAsync(Guid PackageId) =>
        TryCatch(async () =>
        {
            ValidatePackageId(PackageId);

            Package maybePackage =
                await this.storageBroker.SelectPackageByIdAsync(PackageId);

            ValidateStoragePackage(maybePackage, PackageId);

            return await this.storageBroker.DeletePackageAsync(maybePackage);
        });

    }
}
