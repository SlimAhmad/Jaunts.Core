// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Fleets.Exceptions
{
    public class FleetDependencyException : Xeption
    {
        public FleetDependencyException(Xeption innerException)
             : base(message: "Fleet dependency error occurred, contact support.", innerException) { }
        public FleetDependencyException(string message,Xeption innerException)
         : base(message, innerException) { }
    }
}
