// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Providers.Exceptions
{
    public class AlreadyExistsProviderException : Xeption
    {
        public AlreadyExistsProviderException(Exception innerException)
            : base(message: "Provider with the same id already exists.", innerException) { }
        public AlreadyExistsProviderException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
