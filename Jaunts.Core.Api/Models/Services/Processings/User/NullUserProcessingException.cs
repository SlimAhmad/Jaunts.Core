// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.User.Exceptions
{
    public class NullUserProcessingException : Xeption
    {
        public NullUserProcessingException()
            : base(message: "User is null.")
        { }
        public NullUserProcessingException(string message)
            : base(message)
        { }
    }
}
