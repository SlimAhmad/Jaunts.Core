// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Aggregation.Account.Exceptions
{
    public class FailedAccountAggregationServiceException : Xeption
    {
        public FailedAccountAggregationServiceException(Exception innerException)
            : base(message: "Failed Account service error occurred, contact support.",
                  innerException)
        { }
    }
}
