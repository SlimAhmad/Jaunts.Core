// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Fleets.Exceptions
{
    public class InvalidFleetReferenceException : Xeption
    {
        public InvalidFleetReferenceException(Exception innerException)
            : base(message: "Invalid fleet reference error occurred.", innerException)
        { }
        public InvalidFleetReferenceException(string message)
            : base(message)
        { }
    }
}
