// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Providers.Exceptions
{
    public class NotFoundProviderException : Xeption
    {
        public NotFoundProviderException(Guid ProviderId)
            : base(message: $"Couldn't find Provider with id: {ProviderId}.") { }
        public NotFoundProviderException(string message)
            : base(message) { }
    }
}
