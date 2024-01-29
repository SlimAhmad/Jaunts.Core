// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLets.Exceptions
{
    public class InvalidShortLetException : Xeption
    {
        public InvalidShortLetException()
            : base(message: "Invalid shortLet. Please fix the errors and try again.")
        { }
        public InvalidShortLetException(string message)
            : base(message)
        { }
    }
}