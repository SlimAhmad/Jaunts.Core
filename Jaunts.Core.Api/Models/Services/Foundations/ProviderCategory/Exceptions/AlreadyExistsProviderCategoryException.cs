// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderCategorys.Exceptions
{
    public class AlreadyExistsProviderCategoryException : Xeption
    {
        public AlreadyExistsProviderCategoryException(Exception innerException)
            : base(message: "ProviderCategory with the same id already exists.", innerException) { }
        public AlreadyExistsProviderCategoryException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
