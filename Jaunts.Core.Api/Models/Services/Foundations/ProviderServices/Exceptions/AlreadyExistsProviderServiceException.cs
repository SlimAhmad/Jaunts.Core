// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderServices.Exceptions
{
    public class AlreadyExistsProviderServiceException : Xeption
    {
        public AlreadyExistsProviderServiceException(Exception innerException)
            : base(message: "ProviderService with the same id already exists.", innerException) { }
        public AlreadyExistsProviderServiceException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
