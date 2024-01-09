using Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<CustomerAttachment> InsertCustomerAttachmentAsync(
            CustomerAttachment customerAttachment);

        IQueryable<CustomerAttachment> SelectAllCustomerAttachments();

        ValueTask<CustomerAttachment> SelectCustomerAttachmentByIdAsync(
            Guid customerId,
            Guid attachmentId);

        ValueTask<CustomerAttachment> UpdateCustomerAttachmentAsync(
            CustomerAttachment customerAttachment);

        ValueTask<CustomerAttachment> DeleteCustomerAttachmentAsync(
            CustomerAttachment customerAttachment);
    }
}
