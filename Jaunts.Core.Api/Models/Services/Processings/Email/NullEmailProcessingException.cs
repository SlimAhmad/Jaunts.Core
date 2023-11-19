// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Emails.Exceptions
{
    public class NullEmailProcessingException : Xeption
    {
        public NullEmailProcessingException()
            : base(message: "Email is null.")
        { }
    }
}
