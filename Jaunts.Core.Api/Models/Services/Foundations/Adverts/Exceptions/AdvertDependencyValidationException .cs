// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Adverts.Exceptions
{
    public class AdvertDependencyValidationException : Xeption
    {
        public AdvertDependencyValidationException(Xeption innerException)
            : base(message: "Advert dependency validation error occurred, fix the errors.", innerException) { }
        public AdvertDependencyValidationException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}
