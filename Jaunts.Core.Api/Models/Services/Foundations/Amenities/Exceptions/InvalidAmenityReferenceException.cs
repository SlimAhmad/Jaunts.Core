// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Amenities.Exceptions
{
    public class InvalidAmenityReferenceException : Xeption
    {
        public InvalidAmenityReferenceException(Exception innerException)
            : base(message: "Invalid amenity reference error occurred.", innerException)
        { }
        public InvalidAmenityReferenceException(string message)
            : base(message)
        { }
    }
}
