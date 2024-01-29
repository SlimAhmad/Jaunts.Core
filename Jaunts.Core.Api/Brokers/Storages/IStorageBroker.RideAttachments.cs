using Jaunts.Core.Api.Models.Services.Foundations.RideAttachments;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<RideAttachment> InsertRideAttachmentAsync(
            RideAttachment providerAttachment);

        IQueryable<RideAttachment> SelectAllRideAttachments();

        ValueTask<RideAttachment> SelectRideAttachmentByIdAsync(
            Guid providerId,
            Guid attachmentId);

        ValueTask<RideAttachment> UpdateRideAttachmentAsync(
            RideAttachment providerAttachment);

        ValueTask<RideAttachment> DeleteRideAttachmentAsync(
            RideAttachment providerAttachment);
    }
}
