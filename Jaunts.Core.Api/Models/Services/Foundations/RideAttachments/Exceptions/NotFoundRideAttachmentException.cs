// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.RideAttachments.Exceptions
{
    public class NotFoundRideAttachmentException : Xeption
    {

        public NotFoundRideAttachmentException(Guid rideId, Guid attachmentId)
        : base(message: $"Couldn't find attachment with Ride id: {rideId} " +
               $"and attachment id: {attachmentId}.")
        { }
        public NotFoundRideAttachmentException(string message)
            : base(message) { }
    }
}
