// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Amenitys.Exceptions
{
    public class AmenityDependencyException : Xeption
    {
        public AmenityDependencyException(Xeption innerException)
             : base(message: "Amenity dependency error occurred, contact support.", innerException) { }
        public AmenityDependencyException(string message,Xeption innerException)
         : base(message, innerException) { }
    }
}
