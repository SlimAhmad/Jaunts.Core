// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Jwts.Exceptions
{
    public class JwtProcessingDependencyValidationException : Xeption
    {
        public JwtProcessingDependencyValidationException(Xeption innerException)
            : base(message: "Jwt dependency validation error occurred, please try again.",
                innerException)
        { }
    }
}
