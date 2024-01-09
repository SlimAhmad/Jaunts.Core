// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.VacationPackagesAttachments.Exceptions
{
    public class VacationPackagesAttachmentDependencyException : Xeption
    {
        public VacationPackagesAttachmentDependencyException(Exception innerException)
             : base(message: "VacationPackagesAttachment dependency error occurred, contact support.", innerException) { }
        public VacationPackagesAttachmentDependencyException(string message,Exception innerException)
         : base(message, innerException) { }
    }
}
