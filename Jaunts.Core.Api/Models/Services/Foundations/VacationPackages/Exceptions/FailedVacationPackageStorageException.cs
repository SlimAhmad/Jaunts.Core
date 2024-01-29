// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Packages.Exceptions
{
    public class FailedPackageStorageException : Xeption
    {
        public FailedPackageStorageException(Exception innerException)
            : base(message: "Failed Package storage error occurred, please contact support.", innerException)
        { }
        public FailedPackageStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
