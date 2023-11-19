// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Jwt.Exceptions
{
	public class NullJwtException : Xeption
	{
		public NullJwtException()
			: base(message: "Jwt is null.")
		{ }
	}
}
