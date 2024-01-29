// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Amenitys.Exceptions
{
    public class NotFoundAmenityException : Xeption
    {
        public NotFoundAmenityException(Guid AmenityId)
            : base(message: $"Couldn't find Amenity with id: {AmenityId}.") { }
        public NotFoundAmenityException(string message)
            : base(message) { }
    }
}
