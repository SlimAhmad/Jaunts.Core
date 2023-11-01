// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Auth.Exceptions
{
    public class InvalidAuthException : Xeption
    {
        public InvalidAuthException()
            : base(message: "Invalid Auth. Please correct the errors and try again.")
        { }
    }
}
