// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments;

namespace Jaunts.Core.Api.Services.Foundations.PromosOfferAttachments
{
    public interface IPromosOfferAttachmentService
    {
        ValueTask<PromosOfferAttachment> AddPromosOfferAttachmentAsync(PromosOfferAttachment promosOfferAttachment);
        IQueryable<PromosOfferAttachment> RetrieveAllPromosOfferAttachments();
        ValueTask<PromosOfferAttachment> RetrievePromosOfferAttachmentByIdAsync(Guid promosOfferId, Guid attachmentId);
        ValueTask<PromosOfferAttachment> RemovePromosOfferAttachmentByIdAsync(Guid promosOfferId, Guid attachmentId);
    }
}
