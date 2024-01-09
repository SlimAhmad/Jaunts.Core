// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Providers.Exceptions
{
    public class ProviderDependencyValidationException : Xeption
    {
        public ProviderDependencyValidationException(Xeption innerException)
            : base(message: "Provider dependency validation error occurred, fix the errors.", innerException) { }
        public ProviderDependencyValidationException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}
