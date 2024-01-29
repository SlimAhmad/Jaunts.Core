// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderServices.Exceptions
{
    public class LockedProviderServiceException : Xeption
    {
        public LockedProviderServiceException(Exception innerException)
            : base(message: "Locked ProviderService record exception, Please try again later.", innerException) { }
        public LockedProviderServiceException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
