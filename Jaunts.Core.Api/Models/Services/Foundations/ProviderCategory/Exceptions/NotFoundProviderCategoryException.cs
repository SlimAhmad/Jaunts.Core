// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderCategorys.Exceptions
{
    public class NotFoundProviderCategoryException : Xeption
    {
        public NotFoundProviderCategoryException(Guid ProviderCategoryId)
            : base(message: $"Couldn't find ProviderCategory with id: {ProviderCategoryId}.") { }
        public NotFoundProviderCategoryException(string message)
            : base(message) { }
    }
}
