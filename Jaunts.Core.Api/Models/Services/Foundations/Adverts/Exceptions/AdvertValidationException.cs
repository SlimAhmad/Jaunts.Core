﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Adverts.Exceptions
{
    public class AdvertValidationException : Xeption
    {
        public AdvertValidationException(Xeption innerException)
            : base(message: "Advert validation error occurred, please try again.", innerException) { }
        public AdvertValidationException(string message,Exception innerException)
            : base(message, innerException) { }

    }
}