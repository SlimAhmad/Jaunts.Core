// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.VacationPackagesAttachments.Exceptions
{
    public class NullVacationPackagesAttachmentException : Xeption
    {
        public NullVacationPackagesAttachmentException() : base(message: "The VacationPackagesAttachment is null.") { }
        public NullVacationPackagesAttachmentException(string message) : base(message) { }
    }
}
