// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Emails.Exceptions
{
    public class InvalidEmailProcessingException : Xeption
    {
        public InvalidEmailProcessingException()
            : base(message: "Invalid email processing, Please correct the errors and try again.") 
        { }
    }
}
