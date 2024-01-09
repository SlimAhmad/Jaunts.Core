// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLets.Exceptions
{
    public class FailedShortLetServiceException : Xeption
    {
        public FailedShortLetServiceException(Exception innerException)
            : base(message: "Failed ShortLet service error occurred, contact support",
                  innerException)
        { }
        public FailedShortLetServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
