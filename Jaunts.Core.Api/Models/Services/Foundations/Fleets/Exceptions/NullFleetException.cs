// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Fleets.Exceptions
{
    public class NullFleetException : Xeption
    {
        public NullFleetException() : base(message: "The fleet is null.") { }
        public NullFleetException(string message) : base(message) { }
    }
}
