// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Packages.Exceptions
{
    public class PackageServiceException : Xeption
    {
        public PackageServiceException(Xeption innerException)
            : base(message: "Package service error occurred, contact support.", innerException) { }
        public PackageServiceException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}