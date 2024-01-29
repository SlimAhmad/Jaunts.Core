// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderCategorys.Exceptions
{
    public class LockedProviderCategoryException : Xeption
    {
        public LockedProviderCategoryException(Exception innerException)
            : base(message: "Locked ProviderCategory record exception, Please try again later.", innerException) { }
        public LockedProviderCategoryException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
