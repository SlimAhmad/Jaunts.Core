// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors.Exceptions
{
    public class FailedProvidersDirectorServiceException : Xeption
    {
        public FailedProvidersDirectorServiceException(Exception innerException)
            : base(message: "Failed ProvidersDirector service error occurred, contact support",
                  innerException)
        { }
        public FailedProvidersDirectorServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
