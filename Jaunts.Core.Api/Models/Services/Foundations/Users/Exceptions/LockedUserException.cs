// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.User.Exceptions
{
	public class LockedUserException : Xeption
	{
		public LockedUserException(Exception innerException)
			: base(message: "Locked User record exception, please try again later", innerException) { }
	}
}
