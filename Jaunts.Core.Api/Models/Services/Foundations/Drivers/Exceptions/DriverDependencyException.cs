// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Drivers.Exceptions
{
    public class DriverDependencyException : Xeption
    {
        public DriverDependencyException(Xeption innerException)
             : base(message: "Driver dependency error occurred, contact support.", innerException) { }
        public DriverDependencyException(string message,Xeption innerException)
         : base(message, innerException) { }
    }
}
