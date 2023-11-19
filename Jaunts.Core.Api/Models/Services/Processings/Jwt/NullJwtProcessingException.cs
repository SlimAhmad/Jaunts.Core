// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Jwts.Exceptions
{
    public class NullJwtProcessingException : Xeption
    {
        public NullJwtProcessingException()
            : base(message: "Jwt is null.")
        { }
    }
}
