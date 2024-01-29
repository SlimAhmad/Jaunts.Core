// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments;
using Microsoft.EntityFrameworkCore;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void SetFlightDealAttachmentReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FlightDealAttachment>()
                .HasKey(flightDealAttachment =>
                    new { flightDealAttachment.FlightDealId, flightDealAttachment.AttachmentId });

            modelBuilder.Entity<FlightDealAttachment>()
                .HasOne(flightDealAttachment => flightDealAttachment.Attachment)
                .WithOne(attachment => attachment.FlightDealAttachment)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FlightDealAttachment>()
                .HasOne(flightDealAttachment => flightDealAttachment.FlightDeal)
                .WithOne(flightDeal => flightDeal.FlightDealAttachment)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
