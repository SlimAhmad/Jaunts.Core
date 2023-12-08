// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Role.Exceptions
{
    public class FailedRoleProcessingServiceException : Xeption
    {
        public FailedRoleProcessingServiceException(Exception innerException)
            : base(message: "Failed role service occurred, please contact support", innerException)
        { }

        public FailedRoleProcessingServiceException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
