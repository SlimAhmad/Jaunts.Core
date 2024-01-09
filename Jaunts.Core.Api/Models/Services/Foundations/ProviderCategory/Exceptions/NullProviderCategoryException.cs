// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderCategorys.Exceptions
{
    public class NullProviderCategoryException : Xeption
    {
        public NullProviderCategoryException() : base(message: "The ProviderCategory is null.") { }
        public NullProviderCategoryException(string message) : base(message) { }
    }
}
