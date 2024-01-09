using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Jaunts.Core.Api.Models.Services.Foundations.VacationPackagesAttachments;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<VacationPackagesAttachment> VacationPackagesAttachments { get; set; }

        public async ValueTask<VacationPackagesAttachment> InsertVacationPackagesAttachmentAsync(VacationPackagesAttachment vacationPackages)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<VacationPackagesAttachment> vacationPackagesEntityEntry = await broker.VacationPackagesAttachments.AddAsync(entity: vacationPackages);
            await broker.SaveChangesAsync();

            return vacationPackagesEntityEntry.Entity;
        }

        public IQueryable<VacationPackagesAttachment> SelectAllVacationPackagesAttachments() => this.VacationPackagesAttachments;

        public async ValueTask<VacationPackagesAttachment> SelectVacationPackagesAttachmentByIdAsync(
             Guid vacationPackagesId,
             Guid attachmentId)
        {
            var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.VacationPackagesAttachments.FindAsync(vacationPackagesId, attachmentId);
        }

        public async ValueTask<VacationPackagesAttachment> UpdateVacationPackagesAttachmentAsync(VacationPackagesAttachment vacationPackages)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<VacationPackagesAttachment> vacationPackagesEntityEntry = broker.VacationPackagesAttachments.Update(entity: vacationPackages);
            await broker.SaveChangesAsync();

            return vacationPackagesEntityEntry.Entity;
        }

        public async ValueTask<VacationPackagesAttachment> DeleteVacationPackagesAttachmentAsync(VacationPackagesAttachment vacationPackages)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<VacationPackagesAttachment> vacationPackagesEntityEntry = broker.VacationPackagesAttachments.Remove(entity: vacationPackages);
            await broker.SaveChangesAsync();

            return vacationPackagesEntityEntry.Entity;
        }
    }
}
