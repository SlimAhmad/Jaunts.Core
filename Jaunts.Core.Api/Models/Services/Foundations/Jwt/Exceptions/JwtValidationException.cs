// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Jwt.Exceptions
{
	public class JwtValidationException : Xeption
	{
		public JwtValidationException(Xeption innerException)
			: base(message: "Jwt validation errors occurred, please try again.",
				  innerException)
		{ }
        public JwtValidationException(string message,Xeption innerException)
            : base(message,innerException)
        { }
    }
}
