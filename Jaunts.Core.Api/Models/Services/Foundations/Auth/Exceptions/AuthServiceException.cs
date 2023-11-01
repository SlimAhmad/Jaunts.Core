// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Auth.Exceptions
{
    public class AuthServiceException : Xeption
    {
        public AuthServiceException(Exception innerException)
            : base(message: "Auth service error occurred, contact support.", innerException) { }
    }
}
