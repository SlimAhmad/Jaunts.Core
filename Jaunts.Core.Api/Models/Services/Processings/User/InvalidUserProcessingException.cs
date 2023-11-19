// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.User.Exceptions
{
    public class InvalidUserProcessingException : Xeption
    {
        public InvalidUserProcessingException()
            : base(message: "Invalid user, Please correct the errors and try again.") 
        { }
    }
}
