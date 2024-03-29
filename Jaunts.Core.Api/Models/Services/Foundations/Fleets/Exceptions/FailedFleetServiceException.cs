﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Fleets.Exceptions
{
    public class FailedFleetServiceException : Xeption
    {
        public FailedFleetServiceException(Exception innerException)
            : base(message: "Failed fleet service error occurred, contact support.",
                  innerException)
        { }
        public FailedFleetServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
