// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.RideAttachments.Exceptions
{
    public class FailedRideAttachmentStorageException : Xeption
    {
        public FailedRideAttachmentStorageException(Exception innerException)
            : base(message: "Failed RideAttachment storage error occurred, Please contact support.", innerException)
        { }
        public FailedRideAttachmentStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
