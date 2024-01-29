namespace Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments.Exceptions
{
    public class InvalidAdvertAttachmentReferenceException : Exception
    {
    
            public InvalidAdvertAttachmentReferenceException(Exception innerException)
                : base(message: "Invalid guardian attachment reference error occurred.", innerException) { }

        public InvalidAdvertAttachmentReferenceException(string message,Exception innerException)
          : base(message, innerException) { }

    }
}
