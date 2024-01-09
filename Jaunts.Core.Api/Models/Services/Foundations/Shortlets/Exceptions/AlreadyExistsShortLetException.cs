// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLets.Exceptions
{
    public class AlreadyExistsShortLetException : Xeption
    {
        public AlreadyExistsShortLetException(Exception innerException)
            : base(message: "ShortLet with the same id already exists.", innerException) { }
        public AlreadyExistsShortLetException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
