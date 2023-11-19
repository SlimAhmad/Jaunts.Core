// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Jwts.Exceptions
{
    public class JwtProcessingValidationException : Xeption
    {
        public JwtProcessingValidationException(Xeption innerException)
            : base(message: "Jwt validation error occurred, please try again.", innerException)
        { }
    }
}
