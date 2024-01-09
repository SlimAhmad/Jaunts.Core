using Jaunts.Core.Api.Models.Services.Foundations.VacationPackagesAttachments;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<VacationPackagesAttachment> InsertVacationPackagesAttachmentAsync(
            VacationPackagesAttachment vacationPackagesAttachment);

        IQueryable<VacationPackagesAttachment> SelectAllVacationPackagesAttachments();

        ValueTask<VacationPackagesAttachment> SelectVacationPackagesAttachmentByIdAsync(
            Guid vacationPackagesId,
            Guid attachmentId);

        ValueTask<VacationPackagesAttachment> UpdateVacationPackagesAttachmentAsync(
            VacationPackagesAttachment vacationPackagesAttachment);

        ValueTask<VacationPackagesAttachment> DeleteVacationPackagesAttachmentAsync(
            VacationPackagesAttachment vacationPackagesAttachment);
    }
}
