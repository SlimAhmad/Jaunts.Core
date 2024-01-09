// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments;

namespace Jaunts.Core.Api.Services.Foundations.ProviderAttachments
{
    public interface IProviderAttachmentService
    {
        ValueTask<ProviderAttachment> AddVacationPackageAttachmentAsync(ProviderAttachment providerAttachment);
        IQueryable<ProviderAttachment> RetrieveAllVacationPackageAttachments();
        ValueTask<ProviderAttachment> RetrieveVacationPackageAttachmentByIdAsync(Guid guardianId, Guid attachmentId);
        ValueTask<ProviderAttachment> RemoveVacationPackageAttachmentByIdAsync(Guid guardianId, Guid attachmentId);
    }
}
