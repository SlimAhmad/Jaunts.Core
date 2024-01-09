﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments.Exceptions
{
    public class ProviderAttachmentValidationException : Xeption
    {
        public ProviderAttachmentValidationException(Exception innerException)
            : base(message: "Invalid input, contact support.", innerException) { }
        public ProviderAttachmentValidationException(string message,Exception innerException)
            : base(message, innerException) { }

    }
}