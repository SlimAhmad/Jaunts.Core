// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Fleets.Exceptions
{
    public class FailedFleetStorageException : Xeption
    {
        public FailedFleetStorageException(Exception innerException)
            : base(message: "Failed fleet storage error occurred, please contact support.", innerException)
        { }
        public FailedFleetStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
