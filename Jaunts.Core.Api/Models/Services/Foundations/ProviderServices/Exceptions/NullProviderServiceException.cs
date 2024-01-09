// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderServices.Exceptions
{
    public class NullProviderServiceException : Xeption
    {
        public NullProviderServiceException() : base(message: "The ProviderService is null.") { }
        public NullProviderServiceException(string message) : base(message) { }
    }
}
