// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderCategorys.Exceptions
{
    public class FailedProviderCategoryStorageException : Xeption
    {
        public FailedProviderCategoryStorageException(Exception innerException)
            : base(message: "Failed ProviderCategory storage error occurred, please contact support.", innerException)
        { }
        public FailedProviderCategoryStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
