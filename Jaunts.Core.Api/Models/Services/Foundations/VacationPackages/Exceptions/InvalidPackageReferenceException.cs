// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.VacationPackages.Exceptions
{
    public class InvalidPackageReferenceException : Xeption
    {
        public InvalidPackageReferenceException(Exception innerException)
            : base(message: "Invalid package reference error occurred.", innerException)
        { }
        public InvalidPackageReferenceException(string message)
            : base(message)
        { }
    }
}
