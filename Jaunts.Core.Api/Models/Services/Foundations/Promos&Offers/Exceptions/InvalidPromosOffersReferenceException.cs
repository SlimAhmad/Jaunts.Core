// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PromosOfferss.Exceptions
{
    public class InvalidPromosOffersReferenceException : Xeption
    {
        public InvalidPromosOffersReferenceException(Exception innerException)
            : base(message: "Invalid promosOffers reference error occurred.", innerException)
        { }
        public InvalidPromosOffersReferenceException(string message)
            : base(message)
        { }
    }
}
