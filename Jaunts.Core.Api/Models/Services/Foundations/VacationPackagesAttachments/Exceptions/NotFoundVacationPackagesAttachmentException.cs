// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.VacationPackagesAttachments.Exceptions
{
    public class NotFoundVacationPackagesAttachmentException : Xeption
    {

        public NotFoundVacationPackagesAttachmentException(Guid packageId, Guid attachmentId)
        : base(message: $"Couldn't find package attachment with student id: {packageId} " +
               $"and attachment id: {attachmentId}.")
        { }
        public NotFoundVacationPackagesAttachmentException(string message)
            : base(message) { }
    }
}
