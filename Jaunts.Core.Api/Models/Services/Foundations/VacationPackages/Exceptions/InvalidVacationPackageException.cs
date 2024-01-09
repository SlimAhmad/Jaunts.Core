// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.VacationPackages.Exceptions
{
    public class InvalidVacationPackageException : Xeption
    {
        public InvalidVacationPackageException(string parameterName, object parameterValue)
         : base(message: $"Invalid vacationPackage, " +
          $"parameter name: {parameterName}, " +
          $"parameter value: {parameterValue}.")
        { }
        public InvalidVacationPackageException()
            : base(message: "Invalid VacationPackage. Please fix the errors and try again.")
        { }
        public InvalidVacationPackageException(string message)
            : base(message)
        { }
    }
}