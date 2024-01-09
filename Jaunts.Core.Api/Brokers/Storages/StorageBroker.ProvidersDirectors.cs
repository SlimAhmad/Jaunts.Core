using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<ProvidersDirector> ProvidersDirector { get; set; }

        public async ValueTask<ProvidersDirector> InsertProvidersDirectorAsync(ProvidersDirector director)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<ProvidersDirector> directorEntityEntry = await broker.ProvidersDirector.AddAsync(entity: director);
            await broker.SaveChangesAsync();

            return directorEntityEntry.Entity;
        }

        public IQueryable<ProvidersDirector> SelectAllProvidersDirectors() => this.ProvidersDirector;

        public async ValueTask<ProvidersDirector> SelectProvidersDirectorByIdAsync(Guid directorId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await ProvidersDirector.FindAsync(directorId);
        }

        public async ValueTask<ProvidersDirector> UpdateProvidersDirectorAsync(ProvidersDirector director)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<ProvidersDirector> directorEntityEntry = broker.ProvidersDirector.Update(entity: director);
            await broker.SaveChangesAsync();

            return directorEntityEntry.Entity;
        }

        public async ValueTask<ProvidersDirector> DeleteProvidersDirectorAsync(ProvidersDirector director)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<ProvidersDirector> directorEntityEntry = broker.ProvidersDirector.Remove(entity: director);
            await broker.SaveChangesAsync();

            return directorEntityEntry.Entity;
        }
    }
}
