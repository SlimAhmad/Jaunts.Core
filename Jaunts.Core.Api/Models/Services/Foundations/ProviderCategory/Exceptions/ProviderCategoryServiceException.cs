// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderCategorys.Exceptions
{
    public class ProviderCategoryServiceException : Xeption
    {
        public ProviderCategoryServiceException(Xeption innerException)
            : base(message: "ProviderCategory service error occurred, contact support.", innerException) { }
        public ProviderCategoryServiceException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}