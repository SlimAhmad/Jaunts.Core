// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.VacationPackagesAttachments.Exceptions
{
    public class LockedVacationPackagesAttachmentException : Xeption
    {
        public LockedVacationPackagesAttachmentException(Exception innerException)
            : base(message: "Locked VacationPackagesAttachment record exception, please try again later.", innerException) { }
        public LockedVacationPackagesAttachmentException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
