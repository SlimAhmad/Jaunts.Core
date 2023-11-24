// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.User.Exceptions
{
	public class NotFoundUserException : Xeption
	{
		public NotFoundUserException(Guid UserId)
			: base(message: $"Couldn't find User with id: {UserId}.")
		{ }
        public NotFoundUserException(string message)
        : base(message) { }
    }
}
