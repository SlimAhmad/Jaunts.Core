// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Packages.Exceptions
{
    public class InvalidPackageException : Xeption
    {
        public InvalidPackageException()
            : base(message: "Invalid Package. Please fix the errors and try again.")
        { }
        public InvalidPackageException(string message)
            : base(message)
        { }
    }
}