// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments;
using Microsoft.EntityFrameworkCore;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void SetDriverAttachmentReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DriverAttachment>()
                .HasKey(driverAttachment =>
                    new { driverAttachment.DriverId, driverAttachment.AttachmentId });

            modelBuilder.Entity<DriverAttachment>()
                .HasOne(driverAttachment => driverAttachment.Attachment)
                .WithMany(attachment => attachment.DriverAttachments)
                .HasForeignKey(driverAttachment => driverAttachment.AttachmentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DriverAttachment>()
                .HasOne(driverAttachment => driverAttachment.Driver)
                .WithMany(driver => driver.DriverAttachments)
                .HasForeignKey(driverAttachment => driverAttachment.DriverId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
