// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Fleets.Exceptions
{
    public class AlreadyExistsFleetException : Xeption
    {
        public AlreadyExistsFleetException(Exception innerException)
            : base(message: "Fleet with the same id already exists.", innerException) { }
        public AlreadyExistsFleetException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
