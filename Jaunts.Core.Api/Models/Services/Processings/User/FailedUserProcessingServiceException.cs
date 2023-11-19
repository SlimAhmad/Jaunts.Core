// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.User.Exceptions
{
    public class FailedUserProcessingServiceException : Xeption
    {
        public FailedUserProcessingServiceException(Exception innerException)
            : base(message: "Failed user service occurred, please contact support", innerException)
        { }
    }
}
