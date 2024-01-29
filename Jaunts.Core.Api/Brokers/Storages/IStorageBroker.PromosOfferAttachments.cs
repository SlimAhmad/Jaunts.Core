using Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<PromosOfferAttachment> InsertPromosOfferAttachmentAsync(
            PromosOfferAttachment promosOfferAttachment);

        IQueryable<PromosOfferAttachment> SelectAllPromosOfferAttachments();

        ValueTask<PromosOfferAttachment> SelectPromosOfferAttachmentByIdAsync(
            Guid promosOfferId,
            Guid attachmentId);

        ValueTask<PromosOfferAttachment> UpdatePromosOfferAttachmentAsync(
            PromosOfferAttachment promosOfferAttachment);

        ValueTask<PromosOfferAttachment> DeletePromosOfferAttachmentAsync(
            PromosOfferAttachment promosOfferAttachment);
    }
}
