// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Role.Exceptions
{
	public class NotFoundRoleException : Xeption
	{
		public NotFoundRoleException(Guid RoleId)
			: base(message: $"Couldn't find Role with id: {RoleId}.")
		{ }
        public NotFoundRoleException(string message)
			: base(message) { }
    }
}
