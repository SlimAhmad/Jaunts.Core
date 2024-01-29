// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Adverts.Exceptions
{
    public class NotFoundAdvertException : Xeption
    {
        public NotFoundAdvertException(Guid advertId)
            : base(message: $"Couldn't find advert with id: {advertId}.") { }
        public NotFoundAdvertException(string message)
            : base(message) { }
    }
}
