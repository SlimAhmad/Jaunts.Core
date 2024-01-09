// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Providers.Exceptions
{
    public class NullProviderException : Xeption
    {
        public NullProviderException() : base(message: "The Provider is null.") { }
        public NullProviderException(string message) : base(message) { }
    }
}
