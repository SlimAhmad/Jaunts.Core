﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLets.Exceptions
{
    public class ShortLetValidationException : Xeption
    {
        public ShortLetValidationException(Xeption innerException)
            : base(message: "ShortLet validation error occurred, please try again.", innerException) { }
        public ShortLetValidationException(string message,Xeption innerException)
            : base(message, innerException) { }

    }
}