// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments;
using Microsoft.EntityFrameworkCore;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void SetPackageAttachmentReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PackageAttachment>()
                .HasKey(PackageAttachment =>
                    new { PackageAttachment.PackageId, PackageAttachment.AttachmentId });

            modelBuilder.Entity<PackageAttachment>()
                .HasOne(PackageAttachment => PackageAttachment.Attachment)
                .WithMany(attachment => attachment.PackageAttachments)
                .HasForeignKey(PackageAttachment => PackageAttachment.AttachmentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PackageAttachment>()
                .HasOne(PackageAttachment => PackageAttachment.Package)
                .WithMany(Package => Package.PackageAttachments)
                .HasForeignKey(PackageAttachment => PackageAttachment.PackageId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
