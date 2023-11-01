// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Role.Exceptions
{
	public class InvalidRoleException : Xeption
	{
		public InvalidRoleException()
			: base(message: "Invalid Role. Please correct the errors and try again.")
		{ }
	}
}
