// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments;
using Microsoft.EntityFrameworkCore;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void SetAdvertAttachmentReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdvertAttachment>()
                .HasKey(advertAttachment =>
                    new { advertAttachment.AdvertId, advertAttachment.AttachmentId });

            modelBuilder.Entity<AdvertAttachment>()
                .HasOne(advertAttachment => advertAttachment.Attachment)
                .WithMany(attachment => attachment.AdvertAttachments)
                .HasForeignKey(advertAttachment => advertAttachment.AttachmentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AdvertAttachment>()
                .HasOne(advertAttachment => advertAttachment.Advert)
                .WithMany(advert => advert.AdvertAttachments)
                .HasForeignKey(advertAttachment => advertAttachment.AdvertId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
