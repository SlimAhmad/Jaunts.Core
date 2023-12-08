// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.SignIns.Exceptions
{
    public class SignInProcessingServiceException : Xeption
    {
        public SignInProcessingServiceException(Exception innerException)
            : base(message: "Failed signIn service occurred, please contact support", innerException)
        { }

        public SignInProcessingServiceException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
