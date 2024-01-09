// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderServices.Exceptions
{
    public class NotFoundProviderServiceException : Xeption
    {
        public NotFoundProviderServiceException(Guid ProviderServiceId)
            : base(message: $"Couldn't find ProviderService with id: {ProviderServiceId}.") { }
        public NotFoundProviderServiceException(string message)
            : base(message) { }
    }
}
