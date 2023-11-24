// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Auth.Exceptions
{
    public class AlreadyExistsAccountException : Xeption
    {
        public AlreadyExistsAccountException(Exception innerException)
            : base(message: "Auth with the same id already exists.", innerException)
        { }

        public AlreadyExistsAccountException(string message,Exception innerException)
            : base(message) { }
    }
}
