// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.User.Exceptions
{
	public class UserValidationException : Xeption
	{
		public UserValidationException(Xeption innerException)
			: base(message: "User validation errors occurred, please try again.",
				  innerException)
		{ }
        public UserValidationException(string message, Exception innerException)
        : base(message, innerException) { }
    }
}
