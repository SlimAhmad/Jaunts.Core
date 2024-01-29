using Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<AdvertAttachment> InsertAdvertAttachmentAsync(
            AdvertAttachment advertAttachment);

        IQueryable<AdvertAttachment> SelectAllAdvertAttachments();

        ValueTask<AdvertAttachment> SelectAdvertAttachmentByIdAsync(
            Guid advertId,
            Guid attachmentId);

        ValueTask<AdvertAttachment> UpdateAdvertAttachmentAsync(
            AdvertAttachment advertAttachment);

        ValueTask<AdvertAttachment> DeleteAdvertAttachmentAsync(
            AdvertAttachment advertAttachment);
    }
}
