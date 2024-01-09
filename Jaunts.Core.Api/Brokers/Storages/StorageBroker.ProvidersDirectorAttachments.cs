using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<ProvidersDirectorAttachment> ProvidersDirectorAttachments { get; set; }

        public async ValueTask<ProvidersDirectorAttachment> InsertProvidersDirectorAttachmentAsync(ProvidersDirectorAttachment director)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<ProvidersDirectorAttachment> directorEntityEntry = await broker.ProvidersDirectorAttachments.AddAsync(entity: director);
            await broker.SaveChangesAsync();

            return directorEntityEntry.Entity;
        }

        public IQueryable<ProvidersDirectorAttachment> SelectAllProvidersDirectorAttachments() => this.ProvidersDirectorAttachments;

        public async ValueTask<ProvidersDirectorAttachment> SelectProvidersDirectorAttachmentByIdAsync(
             Guid directorId,
             Guid attachmentId)
        {
            var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.ProvidersDirectorAttachments.FindAsync(directorId, attachmentId);
        }

        public async ValueTask<ProvidersDirectorAttachment> UpdateProvidersDirectorAttachmentAsync(ProvidersDirectorAttachment director)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<ProvidersDirectorAttachment> directorEntityEntry = broker.ProvidersDirectorAttachments.Update(entity: director);
            await broker.SaveChangesAsync();

            return directorEntityEntry.Entity;
        }

        public async ValueTask<ProvidersDirectorAttachment> DeleteProvidersDirectorAttachmentAsync(ProvidersDirectorAttachment director)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<ProvidersDirectorAttachment> directorEntityEntry = broker.ProvidersDirectorAttachments.Remove(entity: director);
            await broker.SaveChangesAsync();

            return directorEntityEntry.Entity;
        }
    }
}
