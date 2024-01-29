// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderCategorys.Exceptions
{
    public class ProviderCategoryValidationException : Xeption
    {
        public ProviderCategoryValidationException(Xeption innerException)
            : base(message: "ProviderCategory validation error occurred, Please try again.", innerException) { }
        public ProviderCategoryValidationException(string message,Xeption innerException)
            : base(message, innerException) { }

    }
}