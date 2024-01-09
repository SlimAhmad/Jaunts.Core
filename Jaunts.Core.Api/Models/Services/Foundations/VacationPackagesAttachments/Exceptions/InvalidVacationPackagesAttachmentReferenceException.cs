namespace Jaunts.Core.Api.Models.Services.Foundations.VacationPackagesAttachments.Exceptions
{
    public class InvalidVacationPackagesAttachmentReferenceException : Exception
    {
    
            public InvalidVacationPackagesAttachmentReferenceException(Exception innerException)
                : base(message: "Invalid guardian attachment reference error occurred.", innerException) { }

        public InvalidVacationPackagesAttachmentReferenceException(string message,Exception innerException)
          : base(message, innerException) { }

    }
}
