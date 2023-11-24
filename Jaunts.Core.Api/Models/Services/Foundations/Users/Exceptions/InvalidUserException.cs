// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.User.Exceptions
{
	public class InvalidUserException : Xeption
	{
		public InvalidUserException()
			: base(message: "Invalid User. Please correct the errors and try again.")
		{ }
        public InvalidUserException(string message)
        : base(message) { }
    }
}
