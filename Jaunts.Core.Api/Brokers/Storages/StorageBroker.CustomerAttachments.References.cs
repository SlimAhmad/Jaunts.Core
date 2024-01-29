// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments;
using Microsoft.EntityFrameworkCore;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void SetCustomerAttachmentReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerAttachment>()
                .HasKey(customerAttachment =>
                    new { customerAttachment.CustomerId, customerAttachment.AttachmentId });

            modelBuilder.Entity<CustomerAttachment>()
                .HasOne(customerAttachment => customerAttachment.Attachment)
                .WithMany(attachment => attachment.CustomerAttachments)
                .HasForeignKey(customerAttachment => customerAttachment.AttachmentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CustomerAttachment>()
                .HasOne(customerAttachment => customerAttachment.Customer)
                .WithMany(customer => customer.CustomerAttachments)
                .HasForeignKey(customerAttachment => customerAttachment.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
