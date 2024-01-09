// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderServices.Exceptions
{
    public class ProviderServiceServiceException : Xeption
    {
        public ProviderServiceServiceException(Xeption innerException)
            : base(message: "ProviderService service error occurred, contact support.", innerException) { }
        public ProviderServiceServiceException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}