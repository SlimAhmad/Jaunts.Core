using Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<ShortLetAttachment> InsertShortLetAttachmentAsync(
            ShortLetAttachment shortLetAttachment);

        IQueryable<ShortLetAttachment> SelectAllShortLetAttachments();

        ValueTask<ShortLetAttachment> SelectShortLetAttachmentByIdAsync(
            Guid shortLetId,
            Guid attachmentId);

        ValueTask<ShortLetAttachment> UpdateShortLetAttachmentAsync(
            ShortLetAttachment shortLetAttachment);

        ValueTask<ShortLetAttachment> DeleteShortLetAttachmentAsync(
            ShortLetAttachment shortLetAttachment);
    }
}
