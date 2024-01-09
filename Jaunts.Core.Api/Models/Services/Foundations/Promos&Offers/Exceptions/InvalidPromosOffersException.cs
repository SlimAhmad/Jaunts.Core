// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PromosOffers.Exceptions
{
    public class InvalidPromosOffersException : Xeption
    {
        public InvalidPromosOffersException(string parameterName, object parameterValue)
            : base(message: $"Invalid PromosOffer, " +
          $"parameter name: {parameterName}, " +
          $"parameter value: {parameterValue}.")
        { }

        public InvalidPromosOffersException()
            : base(message: "Invalid PromosOffers. Please fix the errors and try again.")
        { }
        public InvalidPromosOffersException(string message)
            : base(message)
        { }
    }
}