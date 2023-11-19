// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Jwt.Exceptions
{
	public class JwtDependencyException : Xeption
	{
		public JwtDependencyException(Exception innerException) :
			base(message: "Jwt dependency error occurred, contact support.", innerException)
		{ }
	}
}
