// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.RideAttachments;

namespace Jaunts.Core.Api.Services.Foundations.RideAttachments
{
    public interface IRideAttachmentService
    {
        ValueTask<RideAttachment> AddRideAttachmentAsync(RideAttachment rideAttachment);
        IQueryable<RideAttachment> RetrieveAllRideAttachments();
        ValueTask<RideAttachment> RetrieveRideAttachmentByIdAsync(Guid rideId, Guid attachmentId);
        ValueTask<RideAttachment> RemoveRideAttachmentByIdAsync(Guid rideId, Guid attachmentId);
    }
}
