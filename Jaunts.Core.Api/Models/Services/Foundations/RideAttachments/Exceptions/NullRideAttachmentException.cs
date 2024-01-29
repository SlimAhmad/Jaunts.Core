// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.RideAttachments.Exceptions
{
    public class NullRideAttachmentException : Xeption
    {
        public NullRideAttachmentException() : base(message: "The RideAttachment is null.") { }
        public NullRideAttachmentException(string message) : base(message) { }
    }
}
