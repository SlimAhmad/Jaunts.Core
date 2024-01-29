// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments;
using Microsoft.EntityFrameworkCore;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void SetShortLetAttachmentReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShortLetAttachment>()
                .HasKey(shortLetAttachment =>
                    new { shortLetAttachment.ShortLetId, shortLetAttachment.AttachmentId });

            modelBuilder.Entity<ShortLetAttachment>()
                .HasOne(shortLetAttachment => shortLetAttachment.Attachment)
                .WithMany(attachment => attachment.ShortLetAttachments)
                .HasForeignKey(shortLetAttachment => shortLetAttachment.AttachmentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ShortLetAttachment>()
                .HasOne(shortLetAttachment => shortLetAttachment.ShortLet)
                .WithMany(shortLet => shortLet.ShortLetAttachments)
                .HasForeignKey(shortLetAttachment => shortLetAttachment.ShortLetId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
