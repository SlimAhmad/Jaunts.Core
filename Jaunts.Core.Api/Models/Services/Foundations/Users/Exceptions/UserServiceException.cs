// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.User.Exceptions
{
	public class UserServiceException : Xeption
	{
		public UserServiceException(Exception innerException)
			: base(message: "User service error occurred, contact support.", innerException) { }
        public UserServiceException(string message, Exception innerException)
		: base(message, innerException) { }
    }
}
