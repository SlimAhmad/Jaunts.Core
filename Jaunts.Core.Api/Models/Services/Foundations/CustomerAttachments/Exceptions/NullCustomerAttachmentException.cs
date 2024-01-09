// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments.Exceptions
{
    public class NullCustomerAttachmentException : Xeption
    {
        public NullCustomerAttachmentException() : base(message: "The CustomerAttachment is null.") { }
        public NullCustomerAttachmentException(string message) : base(message) { }
    }
}
