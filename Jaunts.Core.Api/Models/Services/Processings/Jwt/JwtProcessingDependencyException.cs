// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Jwts.Exceptions
{
    public class JwtProcessingDependencyException : Xeption
    {
        public JwtProcessingDependencyException(Xeption innerException)
            : base(message: "Jwt Impression dependency error occurred, please contact support", innerException)
        { }
    }
}
