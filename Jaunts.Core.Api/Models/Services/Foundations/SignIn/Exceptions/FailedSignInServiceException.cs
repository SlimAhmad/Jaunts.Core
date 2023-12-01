// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.SignIn.Exceptions
{
	public class FailedSignInServiceException : Xeption
	{
		public FailedSignInServiceException(Exception innerException)
			: base(message: "Failed SignIn service occurred, please contact support", innerException)
		{ }
        public FailedSignInServiceException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
