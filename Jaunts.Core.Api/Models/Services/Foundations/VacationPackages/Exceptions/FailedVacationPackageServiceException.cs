// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Packages.Exceptions
{
    public class FailedPackageServiceException : Xeption
    {
        public FailedPackageServiceException(Exception innerException)
            : base(message: "Failed Package service error occurred, contact support.",
                  innerException)
        { }
        public FailedPackageServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
