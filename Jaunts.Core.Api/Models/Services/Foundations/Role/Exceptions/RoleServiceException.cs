// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Role.Exceptions
{
	public class RoleServiceException : Xeption
	{
		public RoleServiceException(Exception innerException)
			: base(message: "Role service error occurred, contact support.", innerException) { }
	}
}
