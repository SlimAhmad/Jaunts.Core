// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Role.Exceptions
{
	public class RoleValidationException : Xeption
	{
		public RoleValidationException(Xeption innerException)
			: base(message: "Role validation errors occurred, please try again.",
				  innerException)
		{ }
	}
}
