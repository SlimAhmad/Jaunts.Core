// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Drivers.Exceptions
{
    public class InvalidDriverReferenceException : Xeption
    {
        public InvalidDriverReferenceException(Exception innerException)
            : base(message: "Invalid driver reference error occurred.", innerException)
        { }
        public InvalidDriverReferenceException(string message)
            : base(message)
        { }
    }
}
