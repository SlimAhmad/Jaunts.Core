namespace Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments.Exceptions
{
    public class InvalidPackageAttachmentReferenceException : Exception
    {
    
            public InvalidPackageAttachmentReferenceException(Exception innerException)
                : base(message: "Invalid guardian attachment reference error occurred.", innerException) { }

        public InvalidPackageAttachmentReferenceException(string message,Exception innerException)
          : base(message, innerException) { }

    }
}
