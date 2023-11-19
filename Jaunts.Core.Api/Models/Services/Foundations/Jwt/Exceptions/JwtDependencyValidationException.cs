// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Jwt.Exceptions
{
	public class JwtDependencyValidationException : Xeption
	{
		public JwtDependencyValidationException(Xeption innerException)
			: base(message: "Jwt dependency validation occurred, please try again.", innerException)
		{ }
	}
}
