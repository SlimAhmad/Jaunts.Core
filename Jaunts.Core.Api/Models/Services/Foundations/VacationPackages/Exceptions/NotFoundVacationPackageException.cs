// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Packages.Exceptions
{
    public class NotFoundPackageException : Xeption
    {
        public NotFoundPackageException(Guid PackageId)
            : base(message: $"Couldn't find Package with id: {PackageId}.") { }
        public NotFoundPackageException(string message)
            : base(message) { }
    }
}
