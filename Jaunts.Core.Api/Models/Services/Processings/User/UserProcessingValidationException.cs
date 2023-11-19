// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.User.Exceptions
{
    public class UserProcessingValidationException : Xeption
    {
        public UserProcessingValidationException(Xeption innerException)
            : base(message: "User validation error occurred, please try again.", innerException)
        { }
    }
}
