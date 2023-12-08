// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Role.Exceptions
{
    public class RoleProcessingDependencyException : Xeption
    {
        public RoleProcessingDependencyException(Xeption innerException)
            : base(message: "Role dependency error occurred, please contact support", innerException)
        { }
        public RoleProcessingDependencyException(string message,Xeption innerException)
            : base(message, innerException)
        { }
    }
}
