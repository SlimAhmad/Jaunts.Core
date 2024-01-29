using Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments;

namespace Jaunts.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<FlightDealAttachment> InsertFlightDealAttachmentAsync(
            FlightDealAttachment flightDealAttachment);

        IQueryable<FlightDealAttachment> SelectAllFlightDealAttachments();

        ValueTask<FlightDealAttachment> SelectFlightDealAttachmentByIdAsync(
            Guid flightDealId,
            Guid attachmentId);

        ValueTask<FlightDealAttachment> UpdateFlightDealAttachmentAsync(
            FlightDealAttachment flightDealAttachment);

        ValueTask<FlightDealAttachment> DeleteFlightDealAttachmentAsync(
            FlightDealAttachment flightDealAttachment);
    }
}
