// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments;

namespace Jaunts.Core.Api.Services.Foundations.ProviderAttachments
{
    public interface IProviderAttachmentService
    {
        ValueTask<ProviderAttachment> AddProviderAttachmentAsync(ProviderAttachment providerAttachment);
        IQueryable<ProviderAttachment> RetrieveAllProviderAttachments();
        ValueTask<ProviderAttachment> RetrieveProviderAttachmentByIdAsync(Guid guardianId, Guid attachmentId);
        ValueTask<ProviderAttachment> RemoveProviderAttachmentByIdAsync(Guid guardianId, Guid attachmentId);
    }
}
