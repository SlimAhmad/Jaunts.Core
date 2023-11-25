// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Role.Exceptions
{
	public class RoleDependencyValidationException : Xeption
	{
		public RoleDependencyValidationException(Xeption innerException)
			: base(message: "Role dependency validation occurred, please try again.", innerException)
		{ }
        public RoleDependencyValidationException(string message, Exception innerException)
			: base(message, innerException) { }
    }
}
