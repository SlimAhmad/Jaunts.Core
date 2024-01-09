// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.VacationPackages.Exceptions
{
    public class FailedVacationPackageStorageException : Xeption
    {
        public FailedVacationPackageStorageException(Exception innerException)
            : base(message: "Failed VacationPackage storage error occurred, please contact support.", innerException)
        { }
        public FailedVacationPackageStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
