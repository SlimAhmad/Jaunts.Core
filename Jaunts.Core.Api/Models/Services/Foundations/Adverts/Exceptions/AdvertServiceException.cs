// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Adverts.Exceptions
{
    public class AdvertServiceException : Xeption
    {
        public AdvertServiceException(Xeption innerException)
            : base(message: "Advert service error occurred, contact support.", innerException) { }
        public AdvertServiceException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}