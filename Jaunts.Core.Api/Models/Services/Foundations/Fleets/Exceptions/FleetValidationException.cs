﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Fleets.Exceptions
{
    public class FleetValidationException : Xeption
    {
        public FleetValidationException(Xeption innerException)
            : base(message: "Fleet validation error occurred, please try again.", innerException) { }
        public FleetValidationException(string message,Xeption innerException)
            : base(message, innerException) { }

    }
}