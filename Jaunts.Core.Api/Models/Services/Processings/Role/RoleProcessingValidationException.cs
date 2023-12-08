// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Role.Exceptions
{
    public class RoleProcessingValidationException : Xeption
    {
        public RoleProcessingValidationException(Xeption innerException)
            : base(message: "Role validation error occurred, please try again.", innerException)
        { }
        public RoleProcessingValidationException(string message,Xeption innerException)
            : base(message, innerException)
        { }
    }
}
