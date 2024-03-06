// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Providers.Exceptions
{
    public class InvalidProviderReferenceException : Xeption
    {
        public InvalidProviderReferenceException(Exception innerException)
            : base(message: "Invalid provider reference error occurred.", innerException)
        { }
        public InvalidProviderReferenceException(string message)
            : base(message)
        { }
    }
}
