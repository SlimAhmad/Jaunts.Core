// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Role.Exceptions
{
    public class InvalidRoleProcessingException : Xeption
    {
        public InvalidRoleProcessingException()
            : base(message: "Invalid Role, Please correct the errors and try again.") 
        { }
        public InvalidRoleProcessingException(string message)
            : base(message)
        { }
    }
}
