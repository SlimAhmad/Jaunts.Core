// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.RideAttachments.Exceptions
{
    public class FailedRideAttachmentServiceException : Xeption
    {
        public FailedRideAttachmentServiceException(Exception innerException)
            : base(message: "Failed RideAttachment service error occurred, Please contact support.",
                  innerException)
        { }
        public FailedRideAttachmentServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
