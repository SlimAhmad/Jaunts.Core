// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Jwt.Exceptions
{
	public class FailedJwtServiceException : Xeption
	{
		public FailedJwtServiceException(Exception innerException)
			: base(message: "Failed Jwt service occurred, please contact support", innerException)
		{ }
        public FailedJwtServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
