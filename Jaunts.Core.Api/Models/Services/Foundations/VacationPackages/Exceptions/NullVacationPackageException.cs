// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Packages.Exceptions
{
    public class NullPackageException : Xeption
    {
        public NullPackageException() : base(message: "The Package is null.") { }
        public NullPackageException(string message) : base(message) { }
    }
}
