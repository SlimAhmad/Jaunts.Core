// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Providers.Exceptions
{
    public class FailedProviderStorageException : Xeption
    {
        public FailedProviderStorageException(Exception innerException)
            : base(message: "Failed Provider storage error occurred, please contact support.", innerException)
        { }
        public FailedProviderStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
