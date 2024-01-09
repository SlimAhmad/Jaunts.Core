// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Adverts.Exceptions
{
    public class FailedAdvertStorageException : Xeption
    {
        public FailedAdvertStorageException(Exception innerException)
            : base(message: "Failed advert storage error occurred, please contact support.", innerException)
        { }
        public FailedAdvertStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
