// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.User.Exceptions
{
	public class InvalidUserReferenceException : Xeption
	{
		public InvalidUserReferenceException(Exception innerException)
			: base(message: "Invalid User reference error occurred.", innerException) { }
	}
}