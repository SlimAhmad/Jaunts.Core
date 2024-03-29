﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Drivers.Exceptions
{
    public class LockedDriverException : Xeption
    {
        public LockedDriverException(Exception innerException)
            : base(message: "Locked driver record exception, please try again later.", innerException) { }
        public LockedDriverException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
