// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Adverts.Exceptions
{
    public class AlreadyExistsAdvertException : Xeption
    {
        public AlreadyExistsAdvertException(Exception innerException)
            : base(message: "Advert with the same id already exists.", innerException) { }
        public AlreadyExistsAdvertException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
