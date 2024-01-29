// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Packages.Exceptions
{
    public class LockedPackageException : Xeption
    {
        public LockedPackageException(Exception innerException)
            : base(message: "Locked Package record exception, please try again later.", innerException) { }
        public LockedPackageException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
