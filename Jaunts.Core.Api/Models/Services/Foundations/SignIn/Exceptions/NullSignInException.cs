// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.SignIn.Exceptions
{
	public class NullSignInException : Xeption
	{
		public NullSignInException()
			: base(message: "SignIn is null.")
		{ }
        public NullSignInException(string message)
            : base(message)
        { }
    }
}
