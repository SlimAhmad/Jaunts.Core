// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments;

namespace Jaunts.Core.Api.Services.Foundations.FlightDealAttachments
{
    public interface IFlightDealAttachmentService
    {
        ValueTask<FlightDealAttachment> AddFlightDealAttachmentAsync(FlightDealAttachment FlightDealAttachment);
        IQueryable<FlightDealAttachment> RetrieveAllFlightDealAttachments();
        ValueTask<FlightDealAttachment> RetrieveFlightDealAttachmentByIdAsync(Guid FlightDealId, Guid attachmentId);
        ValueTask<FlightDealAttachment> RemoveFlightDealAttachmentByIdAsync(Guid FlightDealId, Guid attachmentId);
    }
}
