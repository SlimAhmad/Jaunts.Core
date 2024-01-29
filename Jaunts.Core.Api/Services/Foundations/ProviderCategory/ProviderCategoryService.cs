// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategory;

namespace Jaunts.Core.Api.Services.Foundations.ProviderCategories
{
    public partial class ProviderCategoryService : IProviderCategoryService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ProviderCategoryService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<ProviderCategory> CreateProviderCategoryAsync(ProviderCategory driver) =>
        TryCatch(async () =>
        {
            ValidateProviderCategoryOnRegister(driver);

            return await this.storageBroker.InsertProviderCategoryAsync(driver);
        });

        public IQueryable<ProviderCategory> RetrieveAllProviderCategories() =>
        TryCatch(() => this.storageBroker.SelectAllProviderCategories());

        public ValueTask<ProviderCategory> RetrieveProviderCategoryByIdAsync(Guid driverId) =>
        TryCatch(async () =>
        {
            ValidateProviderCategoryId(driverId);
            ProviderCategory maybeProviderCategory = await this.storageBroker.SelectProviderCategoryByIdAsync(driverId);
            ValidateStorageProviderCategory(maybeProviderCategory, driverId);

            return maybeProviderCategory;
        });

        public ValueTask<ProviderCategory> ModifyProviderCategoryAsync(ProviderCategory driver) =>
        TryCatch(async () =>
        {
            ValidateProviderCategoryOnModify(driver);

            ProviderCategory maybeProviderCategory =
                await this.storageBroker.SelectProviderCategoryByIdAsync(driver.Id);

            ValidateStorageProviderCategory(maybeProviderCategory, driver.Id);
            ValidateAgainstStorageProviderCategoryOnModify(inputProviderCategory: driver, storageProviderCategory: maybeProviderCategory);

            return await this.storageBroker.UpdateProviderCategoryAsync(driver);
        });

        public ValueTask<ProviderCategory> RemoveProviderCategoryByIdAsync(Guid driverId) =>
        TryCatch(async () =>
        {
            ValidateProviderCategoryId(driverId);

            ProviderCategory maybeProviderCategory =
                await this.storageBroker.SelectProviderCategoryByIdAsync(driverId);

            ValidateStorageProviderCategory(maybeProviderCategory, driverId);

            return await this.storageBroker.DeleteProviderCategoryAsync(maybeProviderCategory);
        });

    }
}
