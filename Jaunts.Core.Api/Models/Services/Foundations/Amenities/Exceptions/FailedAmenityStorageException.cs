// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Amenitys.Exceptions
{
    public class FailedAmenityStorageException : Xeption
    {
        public FailedAmenityStorageException(Exception innerException)
            : base(message: "Failed Amenity storage error occurred, please contact support.", innerException)
        { }
        public FailedAmenityStorageException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
