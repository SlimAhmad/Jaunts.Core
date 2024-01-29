// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PromosOffers.Exceptions
{
    public class NullPromosOffersException : Xeption
    {
        public NullPromosOffersException() : base(message: "The PromosOffer is null.") { }
        public NullPromosOffersException(string message) : base(message) { }
    }
}
