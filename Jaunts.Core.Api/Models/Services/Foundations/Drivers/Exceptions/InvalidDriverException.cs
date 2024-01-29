// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Drivers.Exceptions
{
    public class InvalidDriverException : Xeption
    {
        public InvalidDriverException()
            : base(message: "Invalid driver. Please fix the errors and try again.") { }

        public InvalidDriverException(string message)
            : base(message)
        { }
    }
}