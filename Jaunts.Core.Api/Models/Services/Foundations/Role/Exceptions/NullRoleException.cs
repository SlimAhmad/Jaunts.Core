// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Role.Exceptions
{
	public class NullRoleException : Xeption
	{
		public NullRoleException()
			: base(message: "Role is null.")
		{ }
        public NullRoleException(string message)
			: base(message) { }
    }
}
