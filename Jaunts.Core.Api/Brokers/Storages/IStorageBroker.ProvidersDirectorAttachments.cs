using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<ProvidersDirectorAttachment> InsertProvidersDirectorAttachmentAsync(
            ProvidersDirectorAttachment providersDirectorAttachment);

        IQueryable<ProvidersDirectorAttachment> SelectAllProvidersDirectorAttachments();

        ValueTask<ProvidersDirectorAttachment> SelectProvidersDirectorAttachmentByIdAsync(
            Guid providersDirectorId,
            Guid attachmentId);

        ValueTask<ProvidersDirectorAttachment> UpdateProvidersDirectorAttachmentAsync(
            ProvidersDirectorAttachment providersDirectorAttachment);

        ValueTask<ProvidersDirectorAttachment> DeleteProvidersDirectorAttachmentAsync(
            ProvidersDirectorAttachment providersDirectorAttachment);
    }
}
