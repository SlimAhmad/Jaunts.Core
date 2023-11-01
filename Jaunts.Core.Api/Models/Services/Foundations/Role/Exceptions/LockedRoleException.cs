// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Role.Exceptions
{
	public class LockedRoleException : Xeption
	{
		public LockedRoleException(Exception innerException)
			: base(message: "Locked Role record exception, please try again later", innerException) { }
	}
}
