// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Attachments.Exceptions
{
	public class FailedAttachmentStorageException : Xeption
	{
		public FailedAttachmentStorageException(Exception innerException)
			: base(message: "Failed Attachment storage error occurred, contact support.", innerException)
		{ }
        public FailedAttachmentStorageException(string message, Exception innerException)
			: base(message, innerException) { }
    }
}
