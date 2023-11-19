// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.SignIn.Exceptions
{
	public class FailedSignInStorageException : Xeption
	{
		public FailedSignInStorageException(Exception innerException)
			: base(message: "Failed SignIn storage error occurred, contact support.", innerException)
		{ }
	}
}
