// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.SignIns.Exceptions
{
    public class FailedSignInProcessingServiceException : Xeption
    {
        public FailedSignInProcessingServiceException(Exception innerException)
            : base(message: "Failed post impression service occurred, please contact support", innerException)
        { }
    }
}
