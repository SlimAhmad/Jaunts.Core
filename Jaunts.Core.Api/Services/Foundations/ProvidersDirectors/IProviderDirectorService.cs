// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors;

namespace Jaunts.Core.Api.Services.Foundations.ProvidersDirectors
{
    public interface IProvidersDirectorService 
    {
        ValueTask<ProvidersDirector> CreateProvidersDirectorAsync(ProvidersDirector providersDirector);
        IQueryable<ProvidersDirector> RetrieveAllProvidersDirectors();
        ValueTask<ProvidersDirector> RetrieveProvidersDirectorByIdAsync(Guid providersDirectorId);
        ValueTask<ProvidersDirector> ModifyProvidersDirectorAsync(ProvidersDirector providersDirector);
        ValueTask<ProvidersDirector> RemoveProvidersDirectorByIdAsync(Guid providersDirectorId);
    }
}