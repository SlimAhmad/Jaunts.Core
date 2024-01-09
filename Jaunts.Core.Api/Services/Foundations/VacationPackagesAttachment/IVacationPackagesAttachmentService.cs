// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.VacationPackagesAttachments;

namespace Jaunts.Core.Api.Services.Foundations.VacationPackagesAttachments
{
    public interface IVacationPackagesAttachmentService
    {
        ValueTask<VacationPackagesAttachment> AddVacationPackageAttachmentAsync(VacationPackagesAttachment vacationPackagesAttachment);
        IQueryable<VacationPackagesAttachment> RetrieveAllVacationPackageAttachments();
        ValueTask<VacationPackagesAttachment> RetrieveVacationPackageAttachmentByIdAsync(Guid guardianId, Guid attachmentId);
        ValueTask<VacationPackagesAttachment> RemoveVacationPackageAttachmentByIdAsync(Guid guardianId, Guid attachmentId);
    }
}
