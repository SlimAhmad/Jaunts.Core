using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<PackageAttachment> PackageAttachments { get; set; }

        public async ValueTask<PackageAttachment> InsertPackageAttachmentAsync(PackageAttachment Package)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<PackageAttachment> PackageEntityEntry = await broker.PackageAttachments.AddAsync(entity: Package);
            await broker.SaveChangesAsync();

            return PackageEntityEntry.Entity;
        }

        public IQueryable<PackageAttachment> SelectAllPackageAttachments() => this.PackageAttachments;

        public async ValueTask<PackageAttachment> SelectPackageAttachmentByIdAsync(
             Guid PackageId,
             Guid attachmentId)
        {
            var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.PackageAttachments.FindAsync(PackageId, attachmentId);
        }

        public async ValueTask<PackageAttachment> UpdatePackageAttachmentAsync(PackageAttachment Package)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<PackageAttachment> PackageEntityEntry = broker.PackageAttachments.Update(entity: Package);
            await broker.SaveChangesAsync();

            return PackageEntityEntry.Entity;
        }

        public async ValueTask<PackageAttachment> DeletePackageAttachmentAsync(PackageAttachment Package)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<PackageAttachment> PackageEntityEntry = broker.PackageAttachments.Remove(entity: Package);
            await broker.SaveChangesAsync();

            return PackageEntityEntry.Entity;
        }
    }
}
