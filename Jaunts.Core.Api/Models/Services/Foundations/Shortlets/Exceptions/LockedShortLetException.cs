// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLets.Exceptions
{
    public class LockedShortLetException : Xeption
    {
        public LockedShortLetException(Exception innerException)
            : base(message: "Locked ShortLet record exception, please try again later.", innerException) { }
        public LockedShortLetException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
