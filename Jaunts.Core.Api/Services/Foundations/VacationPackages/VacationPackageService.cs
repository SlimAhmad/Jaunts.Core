// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.VacationPackages;

namespace Jaunts.Core.Api.Services.Foundations.VacationPackages
{
    public partial class VacationPackageService : IVacationPackageService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public VacationPackageService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<VacationPackage> RegisterVacationPackageAsync(VacationPackage vacationPackage) =>
        TryCatch(async () =>
        {
            ValidateVacationPackageOnRegister(vacationPackage);

            return await this.storageBroker.InsertVacationPackageAsync(vacationPackage);
        });

        public IQueryable<VacationPackage> RetrieveAllVacationPackages() =>
        TryCatch(() => this.storageBroker.SelectAllVacationPackages());

        public ValueTask<VacationPackage> RetrieveVacationPackageByIdAsync(Guid vacationPackageId) =>
        TryCatch(async () =>
        {
            ValidateVacationPackageId(vacationPackageId);
            VacationPackage maybeVacationPackage = await this.storageBroker.SelectVacationPackageByIdAsync(vacationPackageId);
            ValidateStorageVacationPackage(maybeVacationPackage, vacationPackageId);

            return maybeVacationPackage;
        });

        public ValueTask<VacationPackage> ModifyVacationPackageAsync(VacationPackage vacationPackage) =>
        TryCatch(async () =>
        {
            ValidateVacationPackageOnModify(vacationPackage);

            VacationPackage maybeVacationPackage =
                await this.storageBroker.SelectVacationPackageByIdAsync(vacationPackage.Id);

            ValidateStorageVacationPackage(maybeVacationPackage, vacationPackage.Id);
            ValidateAgainstStorageVacationPackageOnModify(inputVacationPackage: vacationPackage, storageVacationPackage: maybeVacationPackage);

            return await this.storageBroker.UpdateVacationPackageAsync(vacationPackage);
        });

        public ValueTask<VacationPackage> RemoveVacationPackageByIdAsync(Guid vacationPackageId) =>
        TryCatch(async () =>
        {
            ValidateVacationPackageId(vacationPackageId);

            VacationPackage maybeVacationPackage =
                await this.storageBroker.SelectVacationPackageByIdAsync(vacationPackageId);

            ValidateStorageVacationPackage(maybeVacationPackage, vacationPackageId);

            return await this.storageBroker.DeleteVacationPackageAsync(maybeVacationPackage);
        });

    }
}
