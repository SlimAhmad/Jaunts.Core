// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.SignIn.Exceptions
{
	public class SignInServiceException : Xeption
	{
		public SignInServiceException(Exception innerException)
			: base(message: "SignIn service error occurred, contact support.", innerException) { }
	}
}
