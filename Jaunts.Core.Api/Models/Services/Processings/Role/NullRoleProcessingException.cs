// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Role.Exceptions
{
    public class NullRoleProcessingException : Xeption
    {
        public NullRoleProcessingException()
            : base(message: "Role is null.")
        { }
        public NullRoleProcessingException(string message)
            : base(message)
        { }
    }
}
