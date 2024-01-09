// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.VacationPackages.Exceptions
{
    public class VacationPackageServiceException : Xeption
    {
        public VacationPackageServiceException(Xeption innerException)
            : base(message: "VacationPackage service error occurred, contact support.", innerException) { }
        public VacationPackageServiceException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}