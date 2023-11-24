// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.User.Exceptions
{
	public class NullUserException : Xeption
	{
		public NullUserException()
			: base(message: "User is null.")
		{ }
        public NullUserException(string message)
        : base(message) { }

    }
}
