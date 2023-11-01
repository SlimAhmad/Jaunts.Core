// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Role.Exceptions
{
	public class InvalidRoleReferenceException : Xeption
	{
		public InvalidRoleReferenceException(Exception innerException)
			: base(message: "Invalid Role reference error occurred.", innerException) { }
	}
}