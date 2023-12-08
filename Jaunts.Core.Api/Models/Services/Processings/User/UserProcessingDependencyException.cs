// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.User.Exceptions
{
    public class UserProcessingDependencyException : Xeption
    {
        public UserProcessingDependencyException(Xeption innerException)
            : base(message: "User dependency error occurred, please contact support", innerException)
        { }
        public UserProcessingDependencyException(string message,Xeption innerException)
            : base(message, innerException)
        { }
    }
}
