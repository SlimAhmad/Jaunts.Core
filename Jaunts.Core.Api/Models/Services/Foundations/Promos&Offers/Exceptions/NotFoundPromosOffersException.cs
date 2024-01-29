// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PromosOffers.Exceptions
{
    public class NotFoundPromosOffersException : Xeption
    {
        public NotFoundPromosOffersException(Guid PromosOffersId)
            : base(message: $"Couldn't find PromosOffer with id: {PromosOffersId}.") { }
        public NotFoundPromosOffersException(string message)
            : base(message) { }
    }
}
