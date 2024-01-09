// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLets.Exceptions
{
    public class FailedShortLetStorageException : Xeption
    {
        public FailedShortLetStorageException(Exception innerException)
            : base(message: "Failed ShortLet storage error occurred, please contact support.", innerException)
        { }
        public FailedShortLetStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
