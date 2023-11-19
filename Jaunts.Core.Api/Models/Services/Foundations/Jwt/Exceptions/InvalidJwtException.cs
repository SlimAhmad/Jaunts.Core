// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Jwt.Exceptions
{
	public class InvalidJwtException : Xeption
	{
		public InvalidJwtException()
			: base(message: "Invalid Jwt. Please correct the errors and try again.")
		{ }
	}
}
