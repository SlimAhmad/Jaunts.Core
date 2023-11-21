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
            : base(message: "Failed external SignIn service occurred, please contact support", innerException)
        { }
    }
}
