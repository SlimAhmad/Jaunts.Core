// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Amenitys.Exceptions
{
    public class FailedAmenityServiceException : Xeption
    {
        public FailedAmenityServiceException(Exception innerException)
            : base(message: "Failed Amenity service error occurred, Please contact support.",
                  innerException)
        { }
        public FailedAmenityServiceException(string message,Exception innerException)
          : base(message,
                innerException)
        { }
    }
}
