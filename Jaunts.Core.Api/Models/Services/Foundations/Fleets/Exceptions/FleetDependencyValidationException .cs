// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Fleets.Exceptions
{
    public class FleetDependencyValidationException : Xeption
    {
        public FleetDependencyValidationException(Xeption innerException)
            : base(message: "Fleet dependency validation error occurred, fix the errors.", innerException) { }
        public FleetDependencyValidationException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}
