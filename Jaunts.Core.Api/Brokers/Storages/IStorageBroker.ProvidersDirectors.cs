using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<ProvidersDirector> InsertProvidersDirectorAsync(
            ProvidersDirector directors);

        IQueryable<ProvidersDirector> SelectAllProvidersDirectors();

        ValueTask<ProvidersDirector> SelectProvidersDirectorByIdAsync(
            Guid directorId);

        ValueTask<ProvidersDirector> UpdateProvidersDirectorAsync(
            ProvidersDirector directors);

        ValueTask<ProvidersDirector> DeleteProvidersDirectorAsync(
            ProvidersDirector directors);
    }
}
