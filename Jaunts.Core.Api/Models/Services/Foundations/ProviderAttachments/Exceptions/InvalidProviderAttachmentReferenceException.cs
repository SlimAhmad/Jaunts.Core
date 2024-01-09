namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderAttachments.Exceptions
{
    public class InvalidProviderAttachmentReferenceException : Exception
    {
    
            public InvalidProviderAttachmentReferenceException(Exception innerException)
                : base(message: "Invalid guardian attachment reference error occurred.", innerException) { }

        public InvalidProviderAttachmentReferenceException(string message,Exception innerException)
          : base(message, innerException) { }

    }
}
