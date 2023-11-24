// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.User.Exceptions
{
	public class FailedUserStorageException : Xeption
	{
		public FailedUserStorageException(Exception innerException)
			: base(message: "Failed User storage error occurred, contact support.", innerException)
		{ }
        public FailedUserStorageException(string message, Exception innerException)
		: base(message, innerException) { }
    }
}
