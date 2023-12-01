// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Jwt.Exceptions
{
	public class JwtServiceException : Xeption
	{
		public JwtServiceException(Exception innerException)
			: base(message: "Jwt service error occurred, contact support.", innerException) { }
        public JwtServiceException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
