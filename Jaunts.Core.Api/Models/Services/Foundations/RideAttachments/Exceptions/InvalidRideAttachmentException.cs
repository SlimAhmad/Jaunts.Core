// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.RideAttachments.Exceptions
{
    public class InvalidRideAttachmentException : Xeption
    {
        public InvalidRideAttachmentException()
         : base(message: $"Invalid RideAttachment. Please fix the errors and try again.")
        { }

        public InvalidRideAttachmentException(string message)
            : base(message)
        { }
    }
}