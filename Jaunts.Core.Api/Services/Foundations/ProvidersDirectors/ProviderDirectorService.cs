// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors;

namespace Jaunts.Core.Api.Services.Foundations.ProvidersDirectors
{
    public partial class ProvidersDirectorService : IProvidersDirectorService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ProvidersDirectorService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<ProvidersDirector> RegisterProvidersDirectorAsync(ProvidersDirector driver) =>
        TryCatch(async () =>
        {
            ValidateProvidersDirectorOnRegister(driver);

            return await this.storageBroker.InsertProvidersDirectorAsync(driver);
        });

        public IQueryable<ProvidersDirector> RetrieveAllProvidersDirectors() =>
        TryCatch(() => this.storageBroker.SelectAllProvidersDirectors());

        public ValueTask<ProvidersDirector> RetrieveProvidersDirectorByIdAsync(Guid driverId) =>
        TryCatch(async () =>
        {
            ValidateProvidersDirectorId(driverId);
            ProvidersDirector maybeProvidersDirector = await this.storageBroker.SelectProvidersDirectorByIdAsync(driverId);
            ValidateStorageProvidersDirector(maybeProvidersDirector, driverId);

            return maybeProvidersDirector;
        });

        public ValueTask<ProvidersDirector> ModifyProvidersDirectorAsync(ProvidersDirector driver) =>
        TryCatch(async () =>
        {
            ValidateProvidersDirectorOnModify(driver);

            ProvidersDirector maybeProvidersDirector =
                await this.storageBroker.SelectProvidersDirectorByIdAsync(driver.Id);

            ValidateStorageProvidersDirector(maybeProvidersDirector, driver.Id);
            ValidateAgainstStorageProvidersDirectorOnModify(inputProvidersDirector: driver, storageProvidersDirector: maybeProvidersDirector);

            return await this.storageBroker.UpdateProvidersDirectorAsync(driver);
        });

        public ValueTask<ProvidersDirector> RemoveProvidersDirectorByIdAsync(Guid driverId) =>
        TryCatch(async () =>
        {
            ValidateProvidersDirectorId(driverId);

            ProvidersDirector maybeProvidersDirector =
                await this.storageBroker.SelectProvidersDirectorByIdAsync(driverId);

            ValidateStorageProvidersDirector(maybeProvidersDirector, driverId);

            return await this.storageBroker.DeleteProvidersDirectorAsync(maybeProvidersDirector);
        });

    }
}
