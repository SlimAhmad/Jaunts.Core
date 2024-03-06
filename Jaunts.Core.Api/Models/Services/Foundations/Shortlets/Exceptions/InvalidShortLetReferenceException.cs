// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLets.Exceptions
{
    public class InvalidShortLetReferenceException : Xeption
    {
        public InvalidShortLetReferenceException(Exception innerException)
            : base(message: "Invalid shortLet reference error occurred.", innerException)
        { }
        public InvalidShortLetReferenceException(string message)
            : base(message)
        { }
    }
}
