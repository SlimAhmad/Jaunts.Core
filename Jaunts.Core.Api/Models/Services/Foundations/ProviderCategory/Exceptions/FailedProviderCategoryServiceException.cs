// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderCategorys.Exceptions
{
    public class FailedProviderCategoryServiceException : Xeption
    {
        public FailedProviderCategoryServiceException(Exception innerException)
            : base(message: "Failed ProviderCategory service error occurred, contact support",
                  innerException)
        { }
        public FailedProviderCategoryServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
