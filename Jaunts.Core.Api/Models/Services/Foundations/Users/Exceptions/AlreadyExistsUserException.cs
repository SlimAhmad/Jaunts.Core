// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.User.Exceptions
{
	public class AlreadyExistsUserException : Xeption
	{
		public AlreadyExistsUserException(Exception innerException)
			: base(message: "User with the same id already exists.", innerException)
		{ }
	}
}
