// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.User.Exceptions
{
	public class FailedUserServiceException : Xeption
	{
		public FailedUserServiceException(Exception innerException)
			: base(message: "Failed User service occurred, please contact support", innerException)
		{ }
	}
}
