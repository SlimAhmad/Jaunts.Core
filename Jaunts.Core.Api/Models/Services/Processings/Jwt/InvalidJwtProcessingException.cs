// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Jwts.Exceptions
{
    public class InvalidJwtProcessingException : Xeption
    {
        public InvalidJwtProcessingException()
            : base(message: "Invalid Jwt processing, Please correct the errors and try again.") 
        { }
    }
}
