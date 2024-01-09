// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.VacationPackages.Exceptions
{
    public class LockedVacationPackageException : Xeption
    {
        public LockedVacationPackageException(Exception innerException)
            : base(message: "Locked VacationPackage record exception, please try again later.", innerException) { }
        public LockedVacationPackageException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
