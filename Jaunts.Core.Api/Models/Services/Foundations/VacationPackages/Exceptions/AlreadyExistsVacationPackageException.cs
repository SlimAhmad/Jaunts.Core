// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Packages.Exceptions
{
    public class AlreadyExistsPackageException : Xeption
    {
        public AlreadyExistsPackageException(Exception innerException)
            : base(message: "Package with the same id already exists.", innerException) { }
        public AlreadyExistsPackageException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
