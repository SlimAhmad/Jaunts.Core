// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.User.Exceptions
{
    public class UserProcessingDependencyValidationException : Xeption
    {
        public UserProcessingDependencyValidationException(Xeption innerException)
            : base(message: "User dependency validation error occurred, please try again.",
                innerException)
        { }
    }
}
