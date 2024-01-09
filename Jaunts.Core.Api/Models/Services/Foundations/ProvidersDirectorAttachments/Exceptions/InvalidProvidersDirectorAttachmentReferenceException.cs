namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectorAttachments.Exceptions
{
    public class InvalidProvidersDirectorAttachmentReferenceException : Exception
    {
    
            public InvalidProvidersDirectorAttachmentReferenceException(Exception innerException)
                : base(message: "Invalid guardian attachment reference error occurred.", innerException) { }

        public InvalidProvidersDirectorAttachmentReferenceException(string message,Exception innerException)
          : base(message, innerException) { }

    }
}
