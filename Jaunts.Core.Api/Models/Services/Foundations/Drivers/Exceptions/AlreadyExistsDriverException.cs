// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Drivers.Exceptions
{
    public class AlreadyExistsDriverException : Xeption
    {
        public AlreadyExistsDriverException(Exception innerException)
            : base(message: "Driver with the same id already exists.", innerException) { }
        public AlreadyExistsDriverException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
