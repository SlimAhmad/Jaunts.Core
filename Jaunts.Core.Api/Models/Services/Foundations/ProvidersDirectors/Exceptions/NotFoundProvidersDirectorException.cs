// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors.Exceptions
{
    public class NotFoundProvidersDirectorException : Xeption
    {
        public NotFoundProvidersDirectorException(Guid ProvidersDirectorId)
            : base(message: $"Couldn't find ProvidersDirector with id: {ProvidersDirectorId}.") { }
        public NotFoundProvidersDirectorException(string message)
            : base(message) { }
    }
}
