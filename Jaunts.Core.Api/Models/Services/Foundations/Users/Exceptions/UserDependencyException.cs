// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.User.Exceptions
{
	public class UserDependencyException : Xeption
	{
		public UserDependencyException(Exception innerException) :
			base(message: "User dependency error occurred, contact support.", innerException)
		{ }
        public UserDependencyException(string message, Exception innerException)
		: base(message, innerException) { }
    }
}
