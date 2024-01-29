// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments;

namespace Jaunts.Core.Api.Services.Foundations.DriverAttachments
{
    public interface IDriverAttachmentService
    {
        ValueTask<DriverAttachment> AddDriverAttachmentAsync(DriverAttachment DriverAttachment);
        IQueryable<DriverAttachment> RetrieveAllDriverAttachments();
        ValueTask<DriverAttachment> RetrieveDriverAttachmentByIdAsync(Guid driverId, Guid attachmentId);
        ValueTask<DriverAttachment> RemoveDriverAttachmentByIdAsync(Guid driverId, Guid attachmentId);
    }
}
