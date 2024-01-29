// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderServices.Exceptions
{
    public class FailedProviderServiceStorageException : Xeption
    {
        public FailedProviderServiceStorageException(Exception innerException)
            : base(message: "Failed ProviderService storage error occurred, Please contact support.", innerException)
        { }
        public FailedProviderServiceStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
