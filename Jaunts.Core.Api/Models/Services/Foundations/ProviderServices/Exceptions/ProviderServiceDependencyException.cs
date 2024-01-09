// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderServices.Exceptions
{
    public class ProviderServiceDependencyException : Xeption
    {
        public ProviderServiceDependencyException(Xeption innerException)
             : base(message: "ProviderService dependency error occurred, contact support.", innerException) { }
        public ProviderServiceDependencyException(string message,Xeption innerException)
         : base(message, innerException) { }
    }
}
