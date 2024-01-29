namespace Jaunts.Core.Api.Models.Services.Foundations.PromosOfferAttachments.Exceptions
{
    public class InvalidPromosOfferAttachmentReferenceException : Exception
    {
    
            public InvalidPromosOfferAttachmentReferenceException(Exception innerException)
                : base(message: "Invalid guardian attachment reference error occurred.", innerException) { }

        public InvalidPromosOfferAttachmentReferenceException(string message,Exception innerException)
          : base(message, innerException) { }

    }
}
