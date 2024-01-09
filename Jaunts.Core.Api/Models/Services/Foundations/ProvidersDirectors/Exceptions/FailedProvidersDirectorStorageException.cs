// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors.Exceptions
{
    public class FailedProvidersDirectorStorageException : Xeption
    {
        public FailedProvidersDirectorStorageException(Exception innerException)
            : base(message: "Failed ProvidersDirector storage error occurred, please contact support.", innerException)
        { }
        public FailedProvidersDirectorStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
