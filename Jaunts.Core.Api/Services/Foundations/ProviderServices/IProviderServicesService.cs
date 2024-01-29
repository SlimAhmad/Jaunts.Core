// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.ProviderServices;

namespace Jaunts.Core.Api.Services.Foundations.ProviderServices
{
    public interface IProviderServicesService 
    {
        ValueTask<ProviderService> CreateProviderServiceAsync(ProviderService providerService);
        IQueryable<ProviderService> RetrieveAllProviderServices();
        ValueTask<ProviderService> RetrieveProviderServiceByIdAsync(Guid providerServiceId);
        ValueTask<ProviderService> ModifyProviderServiceAsync(ProviderService providerService);
        ValueTask<ProviderService> RemoveProviderServiceByIdAsync(Guid providerServiceId);
    }
}