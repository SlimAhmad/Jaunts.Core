// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments;

namespace Jaunts.Core.Api.Services.Foundations.ProvidersDirectorAttachments
{
    public interface IProvidersDirectorAttachmentService
    {
        ValueTask<ProvidersDirectorAttachment> AddProvidersDirectorAttachmentAsync(ProvidersDirectorAttachment providersDirectorAttachment);
        IQueryable<ProvidersDirectorAttachment> RetrieveAllProvidersDirectorAttachments();
        ValueTask<ProvidersDirectorAttachment> RetrieveProvidersDirectorAttachmentByIdAsync(Guid providersDirectorId, Guid attachmentId);
        ValueTask<ProvidersDirectorAttachment> RemoveProvidersDirectorAttachmentByIdAsync(Guid providersDirectorId, Guid attachmentId);
    }
}
