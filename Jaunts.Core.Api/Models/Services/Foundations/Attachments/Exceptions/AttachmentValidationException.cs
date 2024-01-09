﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions
{
    public class AttachmentValidationException : Exception
    {
        public AttachmentValidationException(Exception innerException)
            : base(message: "Invalid input, contact support.", innerException) { }
    }
}