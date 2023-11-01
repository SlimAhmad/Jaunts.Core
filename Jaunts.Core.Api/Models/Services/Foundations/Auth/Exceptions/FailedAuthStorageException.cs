// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Auth.Exceptions
{
    public class FailedAuthStorageException : Xeption
    {
        public FailedAuthStorageException(Exception innerException)
            : base(message: "Failed Auth storage error occurred, contact support.", innerException)
        { }
    }
}
