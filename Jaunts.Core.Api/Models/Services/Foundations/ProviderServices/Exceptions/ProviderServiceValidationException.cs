// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderServices.Exceptions
{
    public class ProviderServiceValidationException : Xeption
    {
        public ProviderServiceValidationException(Xeption innerException)
            : base(message: "Invalid input, contact support.", innerException) { }
        public ProviderServiceValidationException(string message,Xeption innerException)
            : base(message, innerException) { }

    }
}