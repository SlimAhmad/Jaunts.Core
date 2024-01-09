// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments;

namespace Jaunts.Core.Api.Services.Foundations.CustomerAttachments
{
    public interface ICustomerAttachmentService
    {
        ValueTask<CustomerAttachment> AddVacationPackageAttachmentAsync(CustomerAttachment customerAttachment);
        IQueryable<CustomerAttachment> RetrieveAllVacationPackageAttachments();
        ValueTask<CustomerAttachment> RetrieveVacationPackageAttachmentByIdAsync(Guid  customerId, Guid attachmentId);
        ValueTask<CustomerAttachment> RemoveVacationPackageAttachmentByIdAsync(Guid customerId, Guid attachmentId);
    }
}
