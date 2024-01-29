// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Drivers.Exceptions
{
    public class FailedDriverServiceException : Xeption
    {
        public FailedDriverServiceException(Exception innerException)
            : base(message: "Failed driver service error occurred, contact support.",
                  innerException)
        { }
        public FailedDriverServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
