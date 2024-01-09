// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors.Exceptions
{
    public class ProvidersDirectorDependencyException : Xeption
    {
        public ProvidersDirectorDependencyException(Xeption innerException)
             : base(message: "ProvidersDirector dependency error occurred, contact support.", innerException) { }
        public ProvidersDirectorDependencyException(string message,Xeption innerException)
         : base(message, innerException) { }
    }
}
