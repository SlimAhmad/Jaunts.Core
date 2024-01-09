// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors.Exceptions
{
    public class ProvidersDirectorServiceException : Xeption
    {
        public ProvidersDirectorServiceException(Xeption innerException)
            : base(message: "ProvidersDirector service error occurred, contact support.", innerException) { }
        public ProvidersDirectorServiceException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}