﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions
{
    public class AttachmentDependencyException : Exception
    {
        public AttachmentDependencyException(Exception innerException)
            : base(message: "Attachment dependency error occurred, contact support.", innerException) { }
        public AttachmentDependencyException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
