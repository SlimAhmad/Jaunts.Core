// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Jwts.Exceptions
{
    public class JwtProcessingServiceException : Xeption
    {
        public JwtProcessingServiceException(Exception innerException)
            : base(message: "Failed jwt processing service occurred, please contact support", innerException)
        { }
        public JwtProcessingServiceException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
