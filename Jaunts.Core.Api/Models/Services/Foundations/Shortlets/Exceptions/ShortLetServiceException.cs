// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLets.Exceptions
{
    public class ShortLetServiceException : Xeption
    {
        public ShortLetServiceException(Xeption innerException)
            : base(message: "ShortLet service error occurred, contact support.", innerException) { }
        public ShortLetServiceException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}