// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.RideAttachments.Exceptions
{
    public class LockedRideAttachmentException : Xeption
    {
        public LockedRideAttachmentException(Exception innerException)
            : base(message: "Locked RideAttachment record exception, Please try again later.", innerException) { }
        public LockedRideAttachmentException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
