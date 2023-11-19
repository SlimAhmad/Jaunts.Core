// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.SignIn.Exceptions
{
	public class SignInDependencyException : Xeption
	{
		public SignInDependencyException(Exception innerException) :
			base(message: "SignIn dependency error occurred, contact support.", innerException)
		{ }
	}
}
