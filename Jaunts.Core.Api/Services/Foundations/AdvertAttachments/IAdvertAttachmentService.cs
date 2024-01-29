// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments;

namespace Jaunts.Core.Api.Services.Foundations.AdvertAttachments
{
    public interface IAdvertAttachmentService
    {
        ValueTask<AdvertAttachment> AddAdvertAttachmentAsync(AdvertAttachment AdvertAttachment);
        IQueryable<AdvertAttachment> RetrieveAllAdvertAttachments();
        ValueTask<AdvertAttachment> RetrieveAdvertAttachmentByIdAsync(Guid AdvertId, Guid attachmentId);
        ValueTask<AdvertAttachment> RemoveAdvertAttachmentByIdAsync(Guid AdvertId, Guid attachmentId);
    }
}
