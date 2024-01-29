// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategory;

namespace Jaunts.Core.Api.Services.Foundations.ProviderCategories
{
    public interface IProviderCategoryService
    {
        ValueTask<ProviderCategory> CreateProviderCategoryAsync(ProviderCategory providerCategory);
        IQueryable<ProviderCategory> RetrieveAllProviderCategories();
        ValueTask<ProviderCategory> RetrieveProviderCategoryByIdAsync(Guid providerCategoryId);
        ValueTask<ProviderCategory> ModifyProviderCategoryAsync(ProviderCategory providerCategory);
        ValueTask<ProviderCategory> RemoveProviderCategoryByIdAsync(Guid providerCategoryId);
    }
}