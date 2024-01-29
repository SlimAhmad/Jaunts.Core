namespace Jaunts.Core.Api.Models.Services.Foundations.DriverAttachments.Exceptions
{
    public class InvalidDriverAttachmentReferenceException : Exception
    {
    
            public InvalidDriverAttachmentReferenceException(Exception innerException)
                : base(message: "Invalid guardian attachment reference error occurred.", innerException) { }

        public InvalidDriverAttachmentReferenceException(string message,Exception innerException)
          : base(message, innerException) { }

    }
}
