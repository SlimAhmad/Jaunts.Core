using Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<PackageAttachment> InsertPackageAttachmentAsync(
            PackageAttachment PackageAttachment);

        IQueryable<PackageAttachment> SelectAllPackageAttachments();

        ValueTask<PackageAttachment> SelectPackageAttachmentByIdAsync(
            Guid PackageId,
            Guid attachmentId);

        ValueTask<PackageAttachment> UpdatePackageAttachmentAsync(
            PackageAttachment PackageAttachment);

        ValueTask<PackageAttachment> DeletePackageAttachmentAsync(
            PackageAttachment PackageAttachment);
    }
}
