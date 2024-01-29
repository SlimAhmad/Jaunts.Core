// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PromosOffers.Exceptions
{
    public class AlreadyExistsPromosOffersException : Xeption
    {
        public AlreadyExistsPromosOffersException(Exception innerException)
            : base(message: "PromosOffer with the same id already exists.", innerException) { }
        public AlreadyExistsPromosOffersException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
