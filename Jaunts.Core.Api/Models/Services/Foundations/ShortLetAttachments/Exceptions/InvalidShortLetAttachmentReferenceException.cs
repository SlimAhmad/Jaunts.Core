namespace Jaunts.Core.Api.Models.Services.Foundations.ShortLetAttachments.Exceptions
{
    public class InvalidShortLetAttachmentReferenceException : Exception
    {
    
            public InvalidShortLetAttachmentReferenceException(Exception innerException)
                : base(message: "Invalid ShortLet attachment reference error occurred.", innerException) { }

        public InvalidShortLetAttachmentReferenceException(string message,Exception innerException)
          : base(message, innerException) { }

    }
}
