// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PromosOffers.Exceptions
{
    public class LockedPromosOffersException : Xeption
    {
        public LockedPromosOffersException(Exception innerException)
            : base(message: "Locked PromosOffers record exception, please try again later.", innerException) { }
        public LockedPromosOffersException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
