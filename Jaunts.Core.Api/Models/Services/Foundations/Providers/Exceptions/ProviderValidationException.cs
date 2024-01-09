// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Providers.Exceptions
{
    public class ProviderValidationException : Xeption
    {
        public ProviderValidationException(Xeption innerException)
            : base(message: "Invalid input, contact support.", innerException) { }
        public ProviderValidationException(string message,Xeption innerException)
            : base(message, innerException) { }

    }
}