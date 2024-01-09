// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLets.Exceptions
{
    public class NotFoundShortLetException : Xeption
    {
        public NotFoundShortLetException(Guid ShortLetId)
            : base(message: $"Couldn't find ShortLet with id: {ShortLetId}.") { }
        public NotFoundShortLetException(string message)
            : base(message) { }
    }
}
