// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.VacationPackages.Exceptions
{
    public class NullVacationPackageException : Xeption
    {
        public NullVacationPackageException() : base(message: "The VacationPackage is null.") { }
        public NullVacationPackageException(string message) : base(message) { }
    }
}
