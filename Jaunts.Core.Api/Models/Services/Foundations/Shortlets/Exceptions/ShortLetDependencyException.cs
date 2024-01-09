// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLets.Exceptions
{
    public class ShortLetDependencyException : Xeption
    {
        public ShortLetDependencyException(Xeption innerException)
             : base(message: "ShortLet dependency error occurred, contact support.", innerException) { }
        public ShortLetDependencyException(string message,Xeption innerException)
         : base(message, innerException) { }
    }
}
