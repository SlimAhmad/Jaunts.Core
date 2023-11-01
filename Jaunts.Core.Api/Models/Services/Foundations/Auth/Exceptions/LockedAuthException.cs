// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Auth.Exceptions
{
    public class LockedAuthException : Xeption
    {
        public LockedAuthException(Exception innerException)
            : base(message: "Locked Auth record exception, please try again later", innerException) { }
    }
}
