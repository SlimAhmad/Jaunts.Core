// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.SignIns.Exceptions
{
    public class NullSignInProcessingException : Xeption
    {
        public NullSignInProcessingException()
            : base(message: "SignIn is null.")
        { }
        public NullSignInProcessingException(string message)
            : base(message)
        { }
    }
}
