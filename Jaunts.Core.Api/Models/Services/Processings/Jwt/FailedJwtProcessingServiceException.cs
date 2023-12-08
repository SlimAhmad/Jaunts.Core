// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Jwts.Exceptions
{
    public class FailedJwtProcessingServiceException : Xeption
    {
        public FailedJwtProcessingServiceException(Exception innerException)
            : base(message: "Failed jwt processing service occurred, please contact support", innerException)
        { }

        public FailedJwtProcessingServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
