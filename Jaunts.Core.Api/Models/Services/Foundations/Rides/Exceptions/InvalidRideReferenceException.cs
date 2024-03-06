// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Rides.Exceptions
{
    public class InvalidRideReferenceException : Xeption
    {
        public InvalidRideReferenceException(Exception innerException)
            : base(message: "Invalid ride reference error occurred.", innerException)
        { }
        public InvalidRideReferenceException(string message)
            : base(message)
        { }
    }
}
