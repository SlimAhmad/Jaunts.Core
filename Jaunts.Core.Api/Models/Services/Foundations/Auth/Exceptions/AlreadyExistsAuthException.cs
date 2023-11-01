// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Auth.Exceptions
{
    public class AlreadyExistsAuthException : Xeption
    {
        public AlreadyExistsAuthException(Exception innerException)
            : base(message: "Auth with the same id already exists.", innerException)
        { }
    }
}
