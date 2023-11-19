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
            : base(message: "Failed external Jwt service occurred, please contact support", innerException)
        { }
    }
}
