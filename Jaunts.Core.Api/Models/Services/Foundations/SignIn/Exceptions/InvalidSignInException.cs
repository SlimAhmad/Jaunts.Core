// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.SignIn.Exceptions
{
	public class InvalidSignInException : Xeption
	{
		public InvalidSignInException()
			: base(message: "Invalid SignIn. Please correct the errors and try again.")
		{ }
	}
}
