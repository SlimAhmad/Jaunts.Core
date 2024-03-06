namespace Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments.Exceptions
{
    public class InvalidFlightDealAttachmentReferenceException : Exception
    {
    
            public InvalidFlightDealAttachmentReferenceException(Exception innerException)
                : base(message: "Invalid flightDeal attachment reference error occurred.", innerException) { }

        public InvalidFlightDealAttachmentReferenceException(string message,Exception innerException)
          : base(message, innerException) { }

    }
}
