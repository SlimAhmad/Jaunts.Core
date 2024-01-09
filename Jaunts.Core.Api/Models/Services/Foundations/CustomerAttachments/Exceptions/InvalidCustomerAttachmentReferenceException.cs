namespace Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments.Exceptions
{
    public class InvalidCustomerAttachmentReferenceException : Exception
    {
    
            public InvalidCustomerAttachmentReferenceException(Exception innerException)
                : base(message: "Invalid guardian attachment reference error occurred.", innerException) { }

        public InvalidCustomerAttachmentReferenceException(string message,Exception innerException)
          : base(message, innerException) { }

    }
}
