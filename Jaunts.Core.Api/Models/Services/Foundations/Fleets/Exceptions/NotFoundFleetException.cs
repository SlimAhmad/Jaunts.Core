// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Fleets.Exceptions
{
    public class NotFoundFleetException : Xeption
    {
        public NotFoundFleetException(Guid fleetId)
            : base(message: $"Couldn't find fleet with id: {fleetId}.") { }
        public NotFoundFleetException(string message)
            : base(message) { }
    }
}
