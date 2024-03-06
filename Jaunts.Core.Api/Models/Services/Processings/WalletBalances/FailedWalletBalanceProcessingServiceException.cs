// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.WalletBalance.Exceptions
{
    public class FailedWalletBalanceProcessingServiceException : Xeption
    {
        public FailedWalletBalanceProcessingServiceException(Exception innerException)
            : base(message: "Failed WalletBalance service occurred, Please contact support", innerException)
        { }

        public FailedWalletBalanceProcessingServiceException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
