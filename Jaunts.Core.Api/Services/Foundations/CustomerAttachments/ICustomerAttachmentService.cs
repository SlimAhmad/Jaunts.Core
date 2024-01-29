// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments;

namespace Jaunts.Core.Api.Services.Foundations.CustomerAttachments
{
    public interface ICustomerAttachmentService
    {
        ValueTask<CustomerAttachment> AddCustomerAttachmentAsync(CustomerAttachment customerAttachment);
        IQueryable<CustomerAttachment> RetrieveAllCustomerAttachments();
        ValueTask<CustomerAttachment> RetrieveCustomerAttachmentByIdAsync(Guid  customerId, Guid attachmentId);
        ValueTask<CustomerAttachment> RemoveCustomerAttachmentByIdAsync(Guid customerId, Guid attachmentId);
    }
}
