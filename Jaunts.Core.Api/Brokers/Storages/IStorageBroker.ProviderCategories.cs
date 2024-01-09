using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategory;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<ProviderCategory> InsertProviderCategoryAsync(
            ProviderCategory category);

        IQueryable<ProviderCategory> SelectAllProviderCategories();

        ValueTask<ProviderCategory> SelectProviderCategoryByIdAsync(
            Guid categoryId);

        ValueTask<ProviderCategory> UpdateProviderCategoryAsync(
            ProviderCategory category);

        ValueTask<ProviderCategory> DeleteProviderCategoryAsync(
            ProviderCategory category);
    }
}
