// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Providers.Exceptions
{
    public class LockedProviderException : Xeption
    {
        public LockedProviderException(Exception innerException)
            : base(message: "Locked Provider record exception, please try again later.", innerException) { }
        public LockedProviderException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
