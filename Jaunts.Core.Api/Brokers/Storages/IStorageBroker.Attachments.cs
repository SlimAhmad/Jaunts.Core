using Jaunts.Core.Api.Models.Services.Foundations.Attachments;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Attachment> InsertAttachmentAsync(Attachment advert);
        IQueryable<Attachment> SelectAllAttachments();
        ValueTask<Attachment> SelectAttachmentByIdAsync(Guid advertId);
        ValueTask<Attachment> UpdateAttachmentAsync(Attachment advert);
        ValueTask<Attachment> DeleteAttachmentAsync(Attachment attachment);
    }
}
