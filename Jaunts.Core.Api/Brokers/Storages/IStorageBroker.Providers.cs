using Jaunts.Core.Api.Models.Services.Foundations.Providers;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Provider> InsertProviderAsync(
            Provider provider);

        IQueryable<Provider> SelectAllProviders();

        ValueTask<Provider> SelectProviderByIdAsync(
            Guid categoryId);

        ValueTask<Provider> UpdateProviderAsync(
            Provider provider);

        ValueTask<Provider> DeleteProviderAsync(
            Provider provider);
    }
}
