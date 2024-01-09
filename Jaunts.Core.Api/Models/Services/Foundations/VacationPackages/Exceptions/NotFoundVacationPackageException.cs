// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.VacationPackages.Exceptions
{
    public class NotFoundVacationPackageException : Xeption
    {
        public NotFoundVacationPackageException(Guid VacationPackageId)
            : base(message: $"Couldn't find VacationPackage with id: {VacationPackageId}.") { }
        public NotFoundVacationPackageException(string message)
            : base(message) { }
    }
}
