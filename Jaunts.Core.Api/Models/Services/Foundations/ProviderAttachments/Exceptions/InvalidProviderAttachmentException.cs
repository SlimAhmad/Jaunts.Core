// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments.Exceptions
{
    public class InvalidProviderAttachmentException : Xeption
    {
        public InvalidProviderAttachmentException()
           : base(message: "Invalid providerAttachment. Please correct the errors and try again.")
        { }

        public InvalidProviderAttachmentException(string message)
            : base(message)
        { }
    }
}