using Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<ProviderAttachment> InsertProviderAttachmentAsync(
            ProviderAttachment providerAttachment);

        IQueryable<ProviderAttachment> SelectAllProviderAttachments();

        ValueTask<ProviderAttachment> SelectProviderAttachmentByIdAsync(
            Guid providerId,
            Guid attachmentId);

        ValueTask<ProviderAttachment> UpdateProviderAttachmentAsync(
            ProviderAttachment providerAttachment);

        ValueTask<ProviderAttachment> DeleteProviderAttachmentAsync(
            ProviderAttachment providerAttachment);
    }
}
