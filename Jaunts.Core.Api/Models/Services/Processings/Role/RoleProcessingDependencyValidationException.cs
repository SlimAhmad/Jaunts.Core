// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Role.Exceptions
{
    public class RoleProcessingDependencyValidationException : Xeption
    {
        public RoleProcessingDependencyValidationException(Xeption innerException)
            : base(message: "Role dependency validation error occurred, please try again.",
                innerException)
        { }

        public RoleProcessingDependencyValidationException(string message,Xeption innerException)
            : base(message,innerException)
        { }
    }
}
