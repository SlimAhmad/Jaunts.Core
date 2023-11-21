// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.SignIns.Exceptions
{
    public class InvalidSignInProcessingException : Xeption
    {
        public InvalidSignInProcessingException()
            : base(message: "Invalid SignIn processing, Please correct the errors and try again.") 
        { }
    }
}
