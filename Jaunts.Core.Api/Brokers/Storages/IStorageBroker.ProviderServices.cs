using Jaunts.Core.Api.Models.Services.Foundations.ProviderServices;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<ProviderService> InsertProviderServiceAsync(
            ProviderService service);

        IQueryable<ProviderService> SelectAllProviderServices();

        ValueTask<ProviderService> SelectProviderServiceByIdAsync(
            Guid serviceId);

        ValueTask<ProviderService> UpdateProviderServiceAsync(
            ProviderService service);

        ValueTask<ProviderService> DeleteProviderServiceAsync(
            ProviderService service);
    }
}
