// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Adverts.Exceptions
{
    public class FailedAdvertServiceException : Xeption
    {
        public FailedAdvertServiceException(Exception innerException)
            : base(message: "Failed advert service error occurred, contact support",
                  innerException)
        { }
        public FailedAdvertServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
