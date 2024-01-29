// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Amenitys.Exceptions
{
    public class AmenityServiceException : Xeption
    {
        public AmenityServiceException(Xeption innerException)
            : base(message: "Amenity service error occurred, contact support.", innerException) { }
        public AmenityServiceException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}