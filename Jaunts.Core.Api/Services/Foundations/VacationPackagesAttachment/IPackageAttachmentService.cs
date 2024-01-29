// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments;

namespace Jaunts.Core.Api.Services.Foundations.PackagesAttachments
{
    public interface IPackageAttachmentService
    {
        ValueTask<PackageAttachment> AddPackageAttachmentAsync(PackageAttachment PackageAttachment);
        IQueryable<PackageAttachment> RetrieveAllPackageAttachments();
        ValueTask<PackageAttachment> RetrievePackageAttachmentByIdAsync(Guid packageId, Guid attachmentId);
        ValueTask<PackageAttachment> RemovePackageAttachmentByIdAsync(Guid packageId, Guid attachmentId);
    }
}
