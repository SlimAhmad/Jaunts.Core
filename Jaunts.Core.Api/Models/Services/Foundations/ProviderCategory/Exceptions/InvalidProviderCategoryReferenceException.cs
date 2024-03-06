// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderCategorys.Exceptions
{
    public class InvalidProviderCategoryReferenceException : Xeption
    {
        public InvalidProviderCategoryReferenceException(Exception innerException)
            : base(message: "Invalid providerCategory reference error occurred.", innerException)
        { }
        public InvalidProviderCategoryReferenceException(string message)
            : base(message)
        { }
    }
}
