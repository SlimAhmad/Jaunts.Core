// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.SignIn.Exceptions
{
	public class SignInDependencyValidationException : Xeption
	{
		public SignInDependencyValidationException(Xeption innerException)
			: base(message: "SignIn dependency validation occurred, please try again.", innerException)
		{ }
	}
}
