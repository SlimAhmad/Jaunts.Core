// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments;
using Microsoft.EntityFrameworkCore;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void SetProvidersDirectorAttachmentReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProvidersDirectorAttachment>()
                .HasKey(providersDirectorAttachment =>
                    new { providersDirectorAttachment.ProviderDirectorId, providersDirectorAttachment.AttachmentId });

            modelBuilder.Entity<ProvidersDirectorAttachment>()
                .HasOne(providersDirectorAttachment => providersDirectorAttachment.Attachment)
                .WithMany(attachment => attachment.ProvidersDirectorsAttachments)
                .HasForeignKey(providersDirectorAttachment => providersDirectorAttachment.AttachmentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProvidersDirectorAttachment>()
                .HasOne(providersDirectorAttachment => providersDirectorAttachment.ProviderDirector)
                .WithMany(providersDirector => providersDirector.ProvidersDirectorAttachments)
                .HasForeignKey(providersDirectorAttachment => providersDirectorAttachment.ProviderDirectorId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
