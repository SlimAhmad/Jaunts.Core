// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.VacationPackagesAttachments.Exceptions
{
    public class VacationPackagesAttachmentServiceException : Xeption
    {
        public VacationPackagesAttachmentServiceException(Exception innerException)
            : base(message: "VacationPackagesAttachment service error occurred, contact support.", innerException) { }
        public VacationPackagesAttachmentServiceException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}