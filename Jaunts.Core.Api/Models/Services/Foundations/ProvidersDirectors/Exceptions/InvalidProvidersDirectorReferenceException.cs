// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors.Exceptions
{
    public class InvalidProvidersDirectorReferenceException : Xeption
    {
        public InvalidProvidersDirectorReferenceException(Exception innerException)
            : base(message: "Invalid providersDirector reference error occurred.", innerException)
        { }
        public InvalidProvidersDirectorReferenceException(string message)
            : base(message)
        { }
    }
}
