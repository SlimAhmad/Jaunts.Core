// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Drivers.Exceptions
{
    public class NotFoundDriverException : Xeption
    {
        public NotFoundDriverException(Guid driverId)
            : base(message: $"Couldn't find driver with id: {driverId}.") { }
        public NotFoundDriverException(string message)
            : base(message) { }
    }
}
