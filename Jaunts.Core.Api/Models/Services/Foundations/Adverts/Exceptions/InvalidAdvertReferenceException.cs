// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Adverts.Exceptions
{
    public class InvalidAdvertReferenceException : Xeption
    {
        public InvalidAdvertReferenceException(Exception innerException)
            : base(message: "Invalid advert reference error occurred.", innerException)
        { }
        public InvalidAdvertReferenceException(string message)
            : base(message)
        { }
    }
}
