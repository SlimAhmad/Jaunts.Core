// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors.Exceptions
{
    public class NullProvidersDirectorException : Xeption
    {
        public NullProvidersDirectorException() : base(message: "The ProvidersDirector is null.") { }
        public NullProvidersDirectorException(string message) : base(message) { }
    }
}
