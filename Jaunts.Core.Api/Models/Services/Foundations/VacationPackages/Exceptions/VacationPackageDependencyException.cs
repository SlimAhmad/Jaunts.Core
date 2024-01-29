// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Packages.Exceptions
{
    public class PackageDependencyException : Xeption
    {
        public PackageDependencyException(Xeption innerException)
             : base(message: "Package dependency error occurred, contact support.", innerException) { }
        public PackageDependencyException(string message,Xeption innerException)
         : base(message, innerException) { }
    }
}
