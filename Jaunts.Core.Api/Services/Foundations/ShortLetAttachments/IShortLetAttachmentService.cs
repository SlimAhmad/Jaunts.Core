// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments;

namespace Jaunts.Core.Api.Services.Foundations.ShortLetAttachments
{
    public interface IShortLetAttachmentService
    {
        ValueTask<ShortLetAttachment> AddShortLetAttachmentAsync(ShortLetAttachment shortLetAttachment);
        IQueryable<ShortLetAttachment> RetrieveAllShortLetAttachments();
        ValueTask<ShortLetAttachment> RetrieveShortLetAttachmentByIdAsync(Guid shortLetId, Guid attachmentId);
        ValueTask<ShortLetAttachment> RemoveShortLetAttachmentByIdAsync(Guid shortLetId, Guid attachmentId);
    }
}
