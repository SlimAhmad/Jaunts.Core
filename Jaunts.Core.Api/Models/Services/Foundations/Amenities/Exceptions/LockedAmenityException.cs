// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Amenitys.Exceptions
{
    public class LockedAmenityException : Xeption
    {
        public LockedAmenityException(Exception innerException)
            : base(message: "Locked Amenity record exception, please try again later.", innerException) { }
        public LockedAmenityException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
