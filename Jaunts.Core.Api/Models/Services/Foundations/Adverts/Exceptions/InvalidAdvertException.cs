// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Adverts.Exceptions
{
    public class InvalidAdvertException : Xeption
    {

        public InvalidAdvertException()
            : base(message: "Invalid advert. Please fix the errors and try again.") { }

        public InvalidAdvertException(string message)
        : base(message)
        { }
    }
}