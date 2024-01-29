// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PromosOffers.Exceptions
{
    public class PromosOffersServiceException : Xeption
    {
        public PromosOffersServiceException(Xeption innerException)
            : base(message: "PromosOffer service error occurred, contact support.", innerException) { }
        public PromosOffersServiceException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}