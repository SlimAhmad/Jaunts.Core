// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Packages.Exceptions
{
    public class PackageValidationException : Xeption
    {
        public PackageValidationException(Xeption innerException)
            : base(message: "Package validation error occurred, please try again.", innerException) { }
        public PackageValidationException(string message,Xeption innerException)
            : base(message, innerException) { }

    }
}