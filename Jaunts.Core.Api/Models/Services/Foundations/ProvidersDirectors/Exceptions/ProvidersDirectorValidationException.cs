// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors.Exceptions
{
    public class ProvidersDirectorValidationException : Xeption
    {
        public ProvidersDirectorValidationException(Xeption innerException)
            : base(message: "Invalid input, contact support.", innerException) { }
        public ProvidersDirectorValidationException(string message,Xeption innerException)
            : base(message, innerException) { }

    }
}