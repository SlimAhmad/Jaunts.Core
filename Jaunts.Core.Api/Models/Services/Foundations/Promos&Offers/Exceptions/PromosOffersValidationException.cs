// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PromosOffers.Exceptions
{
    public class PromosOffersValidationException : Xeption
    {
        public PromosOffersValidationException(Xeption innerException)
            : base(message: "Invalid input, contact support.", innerException) { }
        public PromosOffersValidationException(string message,Xeption innerException)
            : base(message, innerException) { }

    }
}