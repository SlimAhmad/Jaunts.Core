// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments;
using Microsoft.EntityFrameworkCore;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void SetPromosOfferAttachmentReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PromosOfferAttachment>()
                .HasKey(promosOfferAttachment =>
                    new { promosOfferAttachment.PromosOfferId, promosOfferAttachment.AttachmentId });

            modelBuilder.Entity<PromosOfferAttachment>()
                .HasOne(promosOfferAttachment => promosOfferAttachment.Attachment)
                .WithOne(attachment => attachment.PromosOfferAttachment)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PromosOfferAttachment>()
                .HasOne(promosOfferAttachment => promosOfferAttachment.PromosOffer)
                .WithOne(promosOffer => promosOffer.PromosOfferAttachment)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
