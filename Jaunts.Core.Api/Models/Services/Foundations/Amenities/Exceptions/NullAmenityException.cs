// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Amenitys.Exceptions
{
    public class NullAmenityException : Xeption
    {
        public NullAmenityException() : base(message: "The Amenity is null.") { }
        public NullAmenityException(string message) : base(message) { }
    }
}
