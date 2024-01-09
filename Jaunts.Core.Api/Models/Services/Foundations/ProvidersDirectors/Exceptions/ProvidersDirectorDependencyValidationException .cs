// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors.Exceptions
{
    public class ProvidersDirectorDependencyValidationException : Xeption
    {
        public ProvidersDirectorDependencyValidationException(Xeption innerException)
            : base(message: "ProvidersDirector dependency validation error occurred, fix the errors.", innerException) { }
        public ProvidersDirectorDependencyValidationException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}
