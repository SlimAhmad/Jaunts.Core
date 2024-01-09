// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderServices.Exceptions
{
    public class FailedProviderServiceServiceException : Xeption
    {
        public FailedProviderServiceServiceException(Exception innerException)
            : base(message: "Failed ProviderService service error occurred, contact support",
                  innerException)
        { }
        public FailedProviderServiceServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
