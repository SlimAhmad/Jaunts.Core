// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Providers.Exceptions
{
    public class FailedProviderServiceException : Xeption
    {
        public FailedProviderServiceException(Exception innerException)
            : base(message: "Failed Provider service error occurred, contact support",
                  innerException)
        { }
        public FailedProviderServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
