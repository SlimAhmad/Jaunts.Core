// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.PromosOffers.Exceptions
{
    public class PromosOffersDependencyException : Xeption
    {
        public PromosOffersDependencyException(Xeption innerException)
             : base(message: "PromosOffers dependency error occurred, contact support.", innerException) { }
        public PromosOffersDependencyException(string message,Xeption innerException)
         : base(message, innerException) { }
    }
}
