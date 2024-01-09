// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments;

namespace Jaunts.Core.Api.Services.Foundations.ProvidersDirectorAttachments
{
    public interface IProvidersDirectorAttachmentService
    {
        ValueTask<ProvidersDirectorAttachment> AddVacationPackageAttachmentAsync(ProvidersDirectorAttachment providersDirectorAttachment);
        IQueryable<ProvidersDirectorAttachment> RetrieveAllVacationPackageAttachments();
        ValueTask<ProvidersDirectorAttachment> RetrieveVacationPackageAttachmentByIdAsync(Guid guardianId, Guid attachmentId);
        ValueTask<ProvidersDirectorAttachment> RemoveVacationPackageAttachmentByIdAsync(Guid guardianId, Guid attachmentId);
    }
}
