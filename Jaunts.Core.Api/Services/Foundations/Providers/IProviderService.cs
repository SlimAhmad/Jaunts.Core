// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Providers;

namespace Jaunts.Core.Api.Services.Foundations.Providers
{
    public interface IProviderService
    {
        ValueTask<Provider> RegisterProviderAsync(Provider provider);
        IQueryable<Provider> RetrieveAllProviders();
        ValueTask<Provider> RetrieveProviderByIdAsync(Guid providerId);
        ValueTask<Provider> ModifyProviderAsync(Provider provider);
        ValueTask<Provider> RemoveProviderByIdAsync(Guid providerId);
    }
}