// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Adverts.Exceptions
{
    public class NullAdvertException : Xeption
    {
        public NullAdvertException() : base(message: "The advert is null.") { }
        public NullAdvertException(string message) : base(message) { }
    }
}
