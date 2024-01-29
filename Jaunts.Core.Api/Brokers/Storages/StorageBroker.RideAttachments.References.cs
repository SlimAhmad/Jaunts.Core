// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.RideAttachments;
using Microsoft.EntityFrameworkCore;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void SetRideAttachmentReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RideAttachment>()
                .HasKey(rideAttachment =>
                    new { rideAttachment.RideId, rideAttachment.AttachmentId });

            modelBuilder.Entity<RideAttachment>()
                .HasOne(rideAttachment => rideAttachment.Attachment)
                .WithMany(attachment => attachment.RideAttachments)
                .HasForeignKey(rideAttachment => rideAttachment.AttachmentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RideAttachment>()
                .HasOne(rideAttachment => rideAttachment.Ride)
                .WithMany(ride => ride.RideAttachments)
                .HasForeignKey(rideAttachment => rideAttachment.RideId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
