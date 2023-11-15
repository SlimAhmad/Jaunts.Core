// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Auth.Exceptions
{
    public class FailedAuthServiceException : Xeption
    {
        public FailedAuthServiceException(Exception innerException)
            : base(message: "Failed Auth service occurred, please contact support", innerException)
        { }

        public FailedAuthServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
