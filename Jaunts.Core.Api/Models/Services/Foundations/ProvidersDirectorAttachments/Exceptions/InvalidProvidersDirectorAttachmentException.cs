// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments.Exceptions
{
    public class InvalidProvidersDirectorAttachmentException : Xeption
    {
        public InvalidProvidersDirectorAttachmentException(string parameterName, object parameterValue)
         : base(message: $"Invalid vacationPackage, " +
          $"parameter name: {parameterName}, " +
          $"parameter value: {parameterValue}.")
        { }

        public InvalidProvidersDirectorAttachmentException(string message)
            : base(message)
        { }
    }
}