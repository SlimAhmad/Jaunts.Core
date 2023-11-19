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
            : base(message: "Failed Role service occurred, please contact support", innerException)
        { }
    }
}
