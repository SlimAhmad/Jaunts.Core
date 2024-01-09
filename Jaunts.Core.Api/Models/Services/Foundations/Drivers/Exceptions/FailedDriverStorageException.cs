// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Drivers.Exceptions
{
    public class FailedDriverStorageException : Xeption
    {
        public FailedDriverStorageException(Exception innerException)
            : base(message: "Failed driver storage error occurred, please contact support.", innerException)
        { }
        public FailedDriverStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
