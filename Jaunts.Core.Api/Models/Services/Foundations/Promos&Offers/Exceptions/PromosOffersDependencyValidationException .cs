// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PromosOffers.Exceptions
{
    public class PromosOffersDependencyValidationException : Xeption
    {
        public PromosOffersDependencyValidationException(Xeption innerException)
            : base(message: "PromosOffer dependency validation error occurred, fix the errors.", innerException) { }
        public PromosOffersDependencyValidationException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}
