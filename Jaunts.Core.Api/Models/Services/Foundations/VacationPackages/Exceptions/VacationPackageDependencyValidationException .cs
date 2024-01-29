// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Packages.Exceptions
{
    public class PackageDependencyValidationException : Xeption
    {
        public PackageDependencyValidationException(Xeption innerException)
            : base(message: "Package dependency validation error occurred, fix the errors.", innerException) { }
        public PackageDependencyValidationException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}
