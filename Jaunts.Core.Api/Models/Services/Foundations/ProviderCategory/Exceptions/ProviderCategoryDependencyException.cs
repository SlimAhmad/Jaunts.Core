// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderCategorys.Exceptions
{
    public class ProviderCategoryDependencyException : Xeption
    {
        public ProviderCategoryDependencyException(Xeption innerException)
             : base(message: "ProviderCategory dependency error occurred, contact support.", innerException) { }
        public ProviderCategoryDependencyException(string message,Xeption innerException)
         : base(message, innerException) { }
    }
}
