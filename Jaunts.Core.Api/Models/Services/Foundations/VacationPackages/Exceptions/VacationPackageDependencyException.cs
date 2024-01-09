// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.VacationPackages.Exceptions
{
    public class VacationPackageDependencyException : Xeption
    {
        public VacationPackageDependencyException(Xeption innerException)
             : base(message: "VacationPackage dependency error occurred, contact support.", innerException) { }
        public VacationPackageDependencyException(string message,Xeption innerException)
         : base(message, innerException) { }
    }
}
