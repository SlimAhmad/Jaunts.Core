// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Role.Exceptions
{
	public class AlreadyExistsRoleException : Xeption
	{
		public AlreadyExistsRoleException(Exception innerException)
			: base(message: "Role with the same id already exists.", innerException)
		{ }
	}
}
