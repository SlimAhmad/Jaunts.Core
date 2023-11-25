// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Role.Exceptions
{
	public class RoleDependencyException : Xeption
	{
		public RoleDependencyException(Exception innerException) :
			base(message: "Role dependency error occurred, contact support.", innerException)
		{ }
        public RoleDependencyException(string message, Exception innerException)
			: base(message, innerException) { }
    }
}
