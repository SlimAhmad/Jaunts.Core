// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PromosOffers.Exceptions
{
    public class FailedPromosOffersServiceException : Xeption
    {
        public FailedPromosOffersServiceException(Exception innerException)
            : base(message: "Failed PromosOffer service error occurred, contact support.",
                  innerException)
        { }
        public FailedPromosOffersServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
