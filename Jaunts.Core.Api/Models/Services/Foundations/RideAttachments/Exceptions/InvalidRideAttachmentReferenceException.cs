namespace Jaunts.Core.Api.Models.Services.Foundations.RideAttachments.Exceptions
{
    public class InvalidRideAttachmentReferenceException : Exception
    {
    
            public InvalidRideAttachmentReferenceException(Exception innerException)
                : base(message: "Invalid ride attachment reference error occurred.", innerException) { }

        public InvalidRideAttachmentReferenceException(string message,Exception innerException)
          : base(message, innerException) { }

    }
}
