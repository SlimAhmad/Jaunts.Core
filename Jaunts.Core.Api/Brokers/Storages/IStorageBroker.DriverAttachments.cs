using Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<DriverAttachment> InsertDriverAttachmentAsync(
            DriverAttachment driverAttachment);

        IQueryable<DriverAttachment> SelectAllDriverAttachments();

        ValueTask<DriverAttachment> SelectDriverAttachmentByIdAsync(
            Guid driverId,
            Guid attachmentId);

        ValueTask<DriverAttachment> UpdateDriverAttachmentAsync(
            DriverAttachment driverAttachment);

        ValueTask<DriverAttachment> DeleteDriverAttachmentAsync(
            DriverAttachment driverAttachment);
    }
}
