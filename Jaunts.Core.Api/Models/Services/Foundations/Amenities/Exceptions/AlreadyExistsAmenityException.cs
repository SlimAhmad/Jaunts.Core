// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Amenitys.Exceptions
{
    public class AlreadyExistsAmenityException : Xeption
    {
        public AlreadyExistsAmenityException(Exception innerException)
            : base(message: "Amenity with the same id already exists.", innerException) { }
        public AlreadyExistsAmenityException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
