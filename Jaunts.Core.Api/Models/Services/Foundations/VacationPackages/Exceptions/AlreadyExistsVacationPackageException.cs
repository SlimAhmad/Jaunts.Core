// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.VacationPackages.Exceptions
{
    public class AlreadyExistsVacationPackageException : Xeption
    {
        public AlreadyExistsVacationPackageException(Exception innerException)
            : base(message: "VacationPackage  with the same id already exists.", innerException) { }
        public AlreadyExistsVacationPackageException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
