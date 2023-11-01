// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Auth.Exceptions
{
    public class InvalidAuthReferenceException : Xeption
    {
        public InvalidAuthReferenceException(Exception innerException)
            : base(message: "Invalid Auth reference error occurred.", innerException) { }
    }
}