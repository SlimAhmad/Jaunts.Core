// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Auth.Exceptions
{
    public class NotFoundAuthException : Xeption
    {
        public NotFoundAuthException(Guid AuthId)
            : base(message: $"Couldn't find Auth with id: {AuthId}.")
        { }
    }
}
