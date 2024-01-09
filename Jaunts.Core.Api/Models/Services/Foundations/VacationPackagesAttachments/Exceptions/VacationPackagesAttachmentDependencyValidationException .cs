// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.VacationPackagesAttachments.Exceptions
{
    public class VacationPackagesAttachmentDependencyValidationException : Xeption
    {
        public VacationPackagesAttachmentDependencyValidationException(Xeption innerException)
            : base(message: "VacationPackagesAttachment dependency validation error occurred, fix the errors.", innerException) { }
        public VacationPackagesAttachmentDependencyValidationException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}
