// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Role.Exceptions
{
	public class FailedRoleStorageException : Xeption
	{
		public FailedRoleStorageException(Exception innerException)
			: base(message: "Failed Role storage error occurred, contact support.", innerException)
		{ }
        public FailedRoleStorageException(string message, Exception innerException)
			: base(message, innerException) { }
    }
}
