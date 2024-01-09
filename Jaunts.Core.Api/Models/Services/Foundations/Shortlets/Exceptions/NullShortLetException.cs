// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLets.Exceptions
{
    public class NullShortLetException : Xeption
    {
        public NullShortLetException() : base(message: "The ShortLet is null.") { }
        public NullShortLetException(string message) : base(message) { }
    }
}
