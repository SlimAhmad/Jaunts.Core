// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments;
using Microsoft.EntityFrameworkCore;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void SetProviderAttachmentReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProviderAttachment>()
                .HasKey(providerAttachment =>
                    new { providerAttachment.ProviderId, providerAttachment.AttachmentId });

            modelBuilder.Entity<ProviderAttachment>()
                .HasOne(providerAttachment => providerAttachment.Attachment)
                .WithMany(attachment => attachment.ProviderAttachments)
                .HasForeignKey(providerAttachment => providerAttachment.AttachmentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProviderAttachment>()
                .HasOne(providerAttachment => providerAttachment.Provider)
                .WithMany(provider => provider.ProviderAttachments)
                .HasForeignKey(providerAttachment => providerAttachment.ProviderId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
