// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderServices.Exceptions
{
    public class InvalidProviderServiceReferenceException : Xeption
    {
        public InvalidProviderServiceReferenceException(Exception innerException)
            : base(message: "Invalid providerService reference error occurred.", innerException)
        { }
        public InvalidProviderServiceReferenceException(string message)
            : base(message)
        { }
    }
}
