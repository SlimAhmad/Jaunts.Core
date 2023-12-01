// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.SignIn.Exceptions
{
	public class SignInValidationException : Xeption
	{
		public SignInValidationException(Xeption innerException)
			: base(message: "SignIn validation errors occurred, please try again.",
				  innerException)
		{ }

        public SignInValidationException(string message,Xeption innerException)
            : base(message,innerException)
        { }
    }
}
