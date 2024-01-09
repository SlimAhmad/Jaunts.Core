﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Providers.Exceptions
{
    public class ProviderServiceException : Xeption
    {
        public ProviderServiceException(Xeption innerException)
            : base(message: "Provider service error occurred, contact support.", innerException) { }
        public ProviderServiceException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}