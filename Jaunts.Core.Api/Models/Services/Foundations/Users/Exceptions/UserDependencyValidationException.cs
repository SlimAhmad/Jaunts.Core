// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.User.Exceptions
{
	public class UserDependencyValidationException : Xeption
	{
		public UserDependencyValidationException(Xeption innerException)
			: base(message: "User dependency validation occurred, please try again.", innerException)
		{ }
	}
}
