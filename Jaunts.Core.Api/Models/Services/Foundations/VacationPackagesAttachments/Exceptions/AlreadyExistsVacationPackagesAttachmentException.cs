// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.VacationPackagesAttachments.Exceptions
{
    public class AlreadyExistsVacationPackagesAttachmentException : Xeption
    {
        public AlreadyExistsVacationPackagesAttachmentException(Exception innerException)
            : base(message: "VacationPackagesAttachment  with the same id already exists.", innerException) { }
        public AlreadyExistsVacationPackagesAttachmentException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
